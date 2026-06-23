using UnityEngine;
using Fungus;

public class FungusTest : MonoBehaviour
{
    public Flowchart flowchart;

    public void Talk()
    {
        flowchart.ExecuteBlock("NPC_Talk");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Talk();
        }
    }
}
