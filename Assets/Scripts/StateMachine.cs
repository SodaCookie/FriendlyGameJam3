using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate string StateTransition(InputState input);

public class StateNode {
	public string name;
	public StateTransition transition = null;

	public StateNode(string name) {
		this.name = name;
	}
}

public class StateMachine {

	StateNode currentNode;
	Dictionary<string, StateNode> nodes = new Dictionary<string, StateNode>();

	public StateMachine(StateNode start) {
		currentNode = start;
	}

	public StateNode CurrentState() {
		return currentNode;
	}

	public void AddNode(StateNode node) {
		nodes.Add (node.name, node);
	}

	public void Process(InputState input) {
		if (currentNode.transition != null) {
			string newNode = currentNode.transition (input);
			if (newNode != null) {
				if (!nodes.ContainsKey (newNode)) {
					Debug.Log ("Not found: " + newNode + " in states");
				} else {
					currentNode = nodes [newNode];
				}
			}
		}
	}
}
