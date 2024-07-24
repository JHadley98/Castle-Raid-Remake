using UnityEngine;

public abstract class State : MonoBehaviour
{
    // Base class for states (allows different scripts to override current state)
    public abstract void RunCurrentState(AIController controller);
}
