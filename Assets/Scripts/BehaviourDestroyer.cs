using UnityEngine;

public class BehaviourDestroyer : MonoBehaviour
{
    public Behaviour m_object;
    public void Destroy()
    {
        if(m_object)
            Destroy(m_object);
    }
}
