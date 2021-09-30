using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

public class ConsoleGUI : MonoBehaviour
{
	public class Log
	{
		public string previewCondition;
		public string condition;
		public string stackTrace;
		public LogType logType;
		public string time;
	}

	public static ConsoleGUI instance { get; private set; }

	public static bool enableInProduction = true;

	[Tooltip("Should the console GUI be persistent between scenes?")]
	[SerializeField] bool dontDestroyOnLoad = true;
	[Tooltip("Key to show and hide the console")]
	[SerializeField] KeyCode showConsoleKey = KeyCode.F4;

	public List<Log> logs = new List<Log>();
	private List<Log> filteredLogs = new List<Log>();

	private Vector2 scrollPos;
	private Vector2 stackTraceScrollPos;
	[Tooltip("Percentage of the screen that the GUI should take up when in minimized mode")]
	[SerializeField]
	private Vector2 minScreenRatio = new Vector2(0.4f, 0.5f);
	private GUIStyle labelStyle;
	private GUIStyle stackTraceLabelStyle;
	private GUIStyle backgroundStyle;
	private GUIStyle toolbarButtonStyle;
	private GUIStyle toolbarToggleStyleOff;
	private GUIStyle toolbarToggleStyleOn;
	private GUIStyle minimiseButtonStyle;

	private GUILayoutOption logLevelWidth = GUILayout.Width(62);
	private GUILayoutOption timeWidth = GUILayout.Width(58);
	private GUILayoutOption toolbarButtonWidth = GUILayout.Width(50);
	private GUILayoutOption toolbarToggleWidth = GUILayout.Width(65);
	private GUILayoutOption toolbarTextFieldWidth = GUILayout.Width(300);

	private Color selectedBackgroundColor = new Color(0.25f, 0.25f, 1f, 1f);
	private Color backgroundColorA = new Color(0.9f, 0.9f, 0.9f, 1);
	private Color backgroundColorB = new Color(1, 1, 1, 1);

	private static readonly Dictionary<LogType, string> logLevelColors = new Dictionary<LogType, string>()
	{
		{ LogType.Error, "red"},
		{ LogType.Exception, "red"},
		{ LogType.Assert, "red"},
		{ LogType.Warning, "yellow"},
		{ LogType.Log, "white"},
	};
	private int selected = -1;
	private float labelHeight;
	private float toolbarHeight = 25;

	private int start;
	private int length;
	private float stackTraceHeight;
	private float consoleHeight;

	private bool showTime = true;

	private bool showLogs = true;
	private bool showWarnings = true;
	private bool showErrors = true;
	private string search = string.Empty;
	private bool filterDirty = true;
	private bool minimise = false;

	[Tooltip("Should the console open automatically if there is an error")]
	public bool forceErrorPopup = false;

	private Rect consoleRect;
	private Rect stackTraceRect;

	Texture2D backgroundImage;
	Texture2D buttonImage;
	Texture2D buttonPressedImage;

	public bool showConsole { get; private set; }

	public ConsoleGUI()
	{
		instance = this;
	}

	private void Awake()
	{
#if !FINAL
		Application.logMessageReceived += LogMessageReceived;
#else
		if(enableInProduction)
			Application.logMessageReceived += LogMessageReceived;
#endif
		if (dontDestroyOnLoad)
			DontDestroyOnLoad(this.gameObject);

		backgroundImage = CreateColorTexture(2, 2, new Color(0.25f, 0.25f, 0.25f, 0.5f));

		buttonImage = CreateColorTexture(2, 2, new Color(0.25f, 0.25f, 0.25f, 0.5f));

		buttonPressedImage = CreateColorTexture(2, 2, new Color(0.45f, 0.45f, 0.45f, 0.5f));
	}

	private Texture2D CreateColorTexture(int _width, int _height, Color _color)
	{
		Texture2D tex = new Texture2D(_width, _height);
		Color[] colors = new Color[_width * _height];
		for (int i = 0; i < colors.Length; ++i)
			colors[i] = _color;
		tex.SetPixels(colors);
		tex.Apply();
		return tex;

	}

	private void OnDestroy()
	{
		Application.logMessageReceived -= LogMessageReceived;
	}

	public void Show()
	{
		showConsole = true;
	}

	public void Hide()
	{
		showConsole = false;
	}

	private void LogMessageReceived(string _condition, string _stackTrace, LogType _type)
	{
		if (enabled)
		{
			var now = System.DateTime.Now;

			int conditionEndLine = _condition.IndexOf('\n');
			if (conditionEndLine < 0)
				conditionEndLine = _condition.Length;
			string previewCondition = _condition.Substring(0, conditionEndLine);

			Log log = new Log()
			{
				previewCondition = previewCondition,
				condition = _condition,
				stackTrace = _stackTrace,
				logType = _type,
				time = $"[{now.Hour}:{now.Minute}:{now.Second}]"
			};

			logs.Add(log);

			int errorFlags = (1 << (int)LogType.Error) | (1 << (int)LogType.Exception) | (1 << (int)LogType.Assert);
			int filterFlag = (showLogs ? 1 << (int)LogType.Log : 0) | (showWarnings ? 1 << (int)LogType.Warning : 0) | (showErrors ? errorFlags : 0);
			if ((string.IsNullOrEmpty(search) ? true : log.condition.Contains(search)) && (((1 << (int)log.logType) & filterFlag) == (1 << (int)log.logType)))
			{
				filteredLogs.Add(log);
			}

			if (forceErrorPopup && (_type == LogType.Error || _type == LogType.Exception))
			{
				showLogs = false;
				showWarnings = false;
				showErrors = true;
				filterDirty = true;
				Show();
			}
		}
	}

	private void InitializeLabelStyle()
	{
		labelStyle = new GUIStyle(GUI.skin.label);
		labelStyle.alignment = TextAnchor.UpperLeft;
		labelStyle.hover.textColor = new Color(0.75f, 0.75f, 0.75f, 1);
		labelStyle.active.textColor = new Color(0.75f, 0.75f, 0.75f, 1);
		labelStyle.wordWrap = false;

		stackTraceLabelStyle = new GUIStyle(labelStyle);
		stackTraceLabelStyle.wordWrap = true;

		labelHeight = labelStyle.CalcHeight(new GUIContent("Example"), 150);

		backgroundStyle = new GUIStyle();
		backgroundStyle.normal.background = backgroundImage;

		toolbarButtonStyle = new GUIStyle(GUI.skin.button);
		toolbarButtonStyle.normal.background = buttonImage;
		toolbarButtonStyle.hover.background = buttonImage;
		toolbarButtonStyle.active.background = buttonPressedImage;

		toolbarToggleStyleOn = new GUIStyle(GUI.skin.button);
		toolbarToggleStyleOn.normal.background = buttonImage;
		toolbarToggleStyleOn.active.background = buttonPressedImage;
		toolbarToggleStyleOn.hover.background = buttonImage;

		toolbarToggleStyleOff = new GUIStyle(GUI.skin.button);
		toolbarToggleStyleOff.normal.background = buttonPressedImage;
		toolbarToggleStyleOff.active.background = buttonPressedImage;
		toolbarToggleStyleOff.hover.background = buttonPressedImage;

		minimiseButtonStyle = new GUIStyle(toolbarButtonStyle);
		minimiseButtonStyle.alignment = TextAnchor.UpperCenter;
	}

	private void UpdateFilter()
	{
		filteredLogs.Clear();

		int errorFlags = (1 << (int)LogType.Error) | (1 << (int)LogType.Exception) | (1 << (int)LogType.Assert);
		int filterFlag = (showLogs ? 1 << (int)LogType.Log : 0) | (showWarnings ? 1 << (int)LogType.Warning : 0) | (showErrors ? errorFlags : 0);

		filteredLogs = logs.FindAll(x => (string.IsNullOrEmpty(search) ? true : x.condition.Contains(search)) && (((1 << (int)x.logType) & filterFlag) == (1 << (int)x.logType)));

		filterDirty = false;
	}

	private void Update()
	{
		stackTraceHeight = Screen.height / 5;
		consoleHeight = Screen.height - stackTraceHeight;
		start = Mathf.Max(0, Mathf.FloorToInt(scrollPos.y / labelHeight) - 1);
		length = Mathf.FloorToInt(consoleHeight / labelHeight);

		if (filterDirty)
		{
			UpdateFilter();
		}

		//Vector2 scroll = Input.mouseScrollDelta;
		//if (consoleRect.Contains(Input.mousePosition))
		//{
		//	scrollPos.y += scroll.y;
		//}
		//else if (stackTraceRect.Contains(Input.mousePosition))
		//{
		//	stackTraceScrollPos.y += scroll.y;
		//}

		if (Input.GetKeyDown(showConsoleKey))
		{
			if (showConsole)
				Hide();
			else
				Show();
		}
	}

	private void OnGUI()
	{
#if FINAL
		if(!enableInProduction)
			return;
#endif

		if (labelStyle == null)
			InitializeLabelStyle();

		if (showConsole)
		{
			Vector2 screenRatio = (minimise ? minScreenRatio : Vector2.one);

			Rect toolbarRect = new Rect(0, Screen.height * (1 - screenRatio.y), Screen.width * screenRatio.x, toolbarHeight);

			GUI.backgroundColor = backgroundColorB;

			GUILayout.BeginArea(toolbarRect, backgroundStyle);

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Clear", toolbarButtonStyle, toolbarButtonWidth))
			{
				logs.Clear();
				filteredLogs.Clear();
				selected = 0;
			}

			if (GUILayout.Button("Time", (showTime ? toolbarToggleStyleOn : toolbarToggleStyleOff), toolbarButtonWidth))
			{
				showTime = !showTime;
			}

			if (GUILayout.Button("Logs", (showLogs ? toolbarToggleStyleOn : toolbarToggleStyleOff), toolbarToggleWidth))
			{
				showLogs = !showLogs;
				filterDirty = true;
			}
			if (GUILayout.Button("Warnings", (showWarnings ? toolbarToggleStyleOn : toolbarToggleStyleOff), toolbarToggleWidth))
			{
				showWarnings = !showWarnings;
				filterDirty = true;
			}
			if (GUILayout.Button("Errors", (showErrors ? toolbarToggleStyleOn : toolbarToggleStyleOff), toolbarToggleWidth))
			{
				showErrors = !showErrors;
				filterDirty = true;
			}

			if (!minimise)
			{
				GUILayout.Space(20);
				GUILayout.Label("Search: ", GUILayout.Width(50));
				int before = search.GetHashCode();
				search = GUILayout.TextField(search, toolbarTextFieldWidth);
				if (search.GetHashCode() != before)
					filterDirty = true;
			}
			GUILayout.FlexibleSpace();

			if (GUILayout.Button((minimise ? "Max" : "Min"), minimiseButtonStyle, toolbarButtonWidth))
			{
				minimise = !minimise;
			}

			if (GUILayout.Button("Close", minimiseButtonStyle, toolbarButtonWidth))
			{
				Hide();
			}

			GUILayout.EndHorizontal();

			GUILayout.EndArea();

			consoleRect = new Rect(0, toolbarRect.yMax, Screen.width * screenRatio.x, consoleHeight * screenRatio.y - (minimise ? labelHeight * 2 : 0));

			GUILayout.BeginArea(consoleRect, backgroundStyle);

			scrollPos = GUILayout.BeginScrollView(scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar);

			GUILayout.BeginVertical();


			GUILayout.Space(start * labelHeight);

			for (int i = start; i < start + length && i < filteredLogs.Count; ++i)
			{
				if (i == selected)
					GUI.backgroundColor = selectedBackgroundColor;
				else if ((i & 1) == 0)
					GUI.backgroundColor = backgroundColorA;
				else
					GUI.backgroundColor = backgroundColorB;

				GUILayout.BeginHorizontal(backgroundStyle);

				if (showTime)
				{
					GUILayout.Label(filteredLogs[i].time, timeWidth);
				}

				labelStyle.alignment = TextAnchor.UpperRight;

				if (GUILayout.Button($"<color={logLevelColors[filteredLogs[i].logType]}>{filteredLogs[i].logType.ToString()}:</color>", labelStyle, logLevelWidth))
					selected = i;

				GUILayout.Space(10);

				labelStyle.alignment = TextAnchor.UpperLeft;

				if (GUILayout.Button(filteredLogs[i].previewCondition, labelStyle))
					selected = i;

				GUILayout.EndHorizontal();
			}

			GUILayout.Space(Mathf.Max(0, filteredLogs.Count - (start + length)) * labelHeight + labelHeight / 2);

			GUILayout.EndVertical();

			GUILayout.EndScrollView();

			GUILayout.EndArea();

			GUI.backgroundColor = backgroundColorA;



			stackTraceRect = new Rect(0, consoleRect.yMax, Screen.width * screenRatio.x, Screen.height - consoleRect.yMax);

			GUILayout.BeginArea(stackTraceRect, backgroundStyle);

			stackTraceScrollPos = GUILayout.BeginScrollView(stackTraceScrollPos);

			GUILayout.BeginVertical(backgroundStyle);
			if (selected < filteredLogs.Count && selected > -1)
				GUILayout.Label($"<color={logLevelColors[filteredLogs[selected].logType]}>{filteredLogs[selected].logType.ToString()}:</color> {filteredLogs[selected].condition}\n{filteredLogs[selected].stackTrace}", stackTraceLabelStyle);

			GUILayout.EndVertical();

			GUILayout.EndScrollView();

			GUILayout.EndArea();
		}
	}

}
