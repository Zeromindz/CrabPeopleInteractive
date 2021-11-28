using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public TreeNode LeftNode { get; set; } = null;
    public TreeNode RightNode { get; set; } = null;
    public TreeNode Parent { get; set; } = null; 
    public LeaderboardData Data { get; set; }

    public bool HasLeft() { return (LeftNode != null); }
    public bool HasRight() { return (RightNode != null); }
    public TreeNode(LeaderboardData data)
	{
        Data = data;
	}
}

public class LeaderBoardBinaryTree : MonoBehaviour
{
    TreeNode root = null;
    private bool IsEmpty() { return (root == null); }
    public void Insert(TreeNode node)
	{
        TreeNode current = root;
        TreeNode parent = null;
        // The tree is empty
        if(current == null)
		{
            root = node;
		}

        while(current != null)
		{
            if(node.Data.playerScore < current.Data.playerScore)
			{
                parent = current;
                current = current.LeftNode;
			}
            else if (node.Data.playerScore > current.Data.playerScore)
			{
                parent = current;
                current = current.RightNode;
			}
            else if (node.Data.playerScore == current.Data.playerScore)
			{
				//Scores are the same
			}
		}

        if (node.Data.playerScore < parent.Data.playerScore)
		{
            parent.LeftNode = node;
		}

        else if(node.Data.playerScore > parent.Data.playerScore)
		{
            parent.RightNode = node;
		}
	}

    public void Remove(float value)
    {
        TreeNode outNode = null;
        TreeNode outParent = null;

        if(FindNode(value, out outNode, out outParent))
		{
            TreeNode currentNode = outNode;
            TreeNode currentParent = outParent;
            TreeNode temp = null;
            TreeNode tempParent = null;

   //         TreeNode rightChild = null;
			//TreeNode leftMostNode = null;
   //         TreeNode leftMostNodeParent = outParent;

			if (currentNode.HasRight())
			{
                temp = currentNode.RightNode;
                tempParent = currentNode;

                while (temp.HasLeft())
				{
                    tempParent = temp;
                    temp = temp.LeftNode;
				}

                currentNode.Data = temp.Data;

                // Deleting parents left node
                if(currentNode == currentParent.LeftNode)
				{
                    //currentParent.LeftNode = temp.RightNode;
                    currentParent.LeftNode = null;
                }

                // Deleting parents right node
                else if(currentNode == currentParent.RightNode)
				{
                    //currentParent.RightNode = temp.RightNode;
                    currentParent.RightNode = null;
                }

            }

            // Current node has no right child
			if (!currentNode.HasRight())
			{
                // Deleting parents left node
                if (currentNode == currentParent.LeftNode)
                {
                    temp = currentNode.LeftNode;
                    currentParent.LeftNode = temp;
                }

                // Deleting parents right node
                else if (currentNode == currentParent.RightNode)
                {
                    temp = currentNode.LeftNode;
                    currentParent.RightNode = temp;
                }

                // Deleting root node
                else if (currentNode == root)
				{
                    root = currentNode.LeftNode;
				}

			}
		}
    }

    /// <summary>
    /// Called when a specific node needs to be found with a value
    /// </summary>
    /// <param name="value">The value being searched</param>
    /// <param name="outNode">The node with the same value</param>
    /// <param name="outParent">The outNodes parent</param>
    /// <returns></returns>
    /// 
    public bool FindNode(float value , out TreeNode outNode, out TreeNode outParent)
	{
		TreeNode current = root;
        outNode = null;
        outParent = null;

		while (current != null)
		{   
            // Value found
            if(value == current.Data.playerScore)
			{
                outNode = current;
                return true;
			}

            // Value is less than the current value
            else if (value < current.Data.playerScore)
			{
                outParent = current;
                current = current.LeftNode;
			}

            // Value is more thanthe current value
            else if (value > current.Data.playerScore)
            {
                outParent = current;
                current = current.RightNode;
            }
        }

        return false;
	}


    public void PrintOrdered()
	{
        PrintDFInOrder(root);
	}

    private void PrintDFInOrder(TreeNode currentNode)
    {
        if (currentNode.LeftNode != null)
        {
            PrintDFInOrder(currentNode.LeftNode);
        }

        Debug.Log(currentNode.Data.playerName + ": " + currentNode.Data.playerScore);


        if (currentNode.RightNode != null)
        {
            PrintDFInOrder(currentNode.RightNode);
        }
    }
}
