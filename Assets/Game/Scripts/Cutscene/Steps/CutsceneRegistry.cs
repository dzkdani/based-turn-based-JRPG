using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Fungus;

public class CutsceneRegistry : MonoBehaviour
{
    public static CutsceneRegistry Instance { get; private set; }

    [SerializeField]
    private List<TimelineRegistryEntry> timelines = new();

    [SerializeField]
    private List<FlowchartRegistryEntry> flowcharts = new();

    private Dictionary<string, PlayableDirector> _timelineLookup;
    private Dictionary<string, Flowchart> _flowchartLookup;

    private void Awake()
    {
        Instance = this;

        _timelineLookup = new();
        _flowchartLookup = new();

        foreach (var entry in timelines)
        {
            if(entry.Director != null)
                _timelineLookup[entry.ID] = entry.Director;
        }

        foreach(var entry in flowcharts)
        {
            if(entry.Flowchart != null)
                _flowchartLookup[entry.ID] = entry.Flowchart;
        }
    }

    public PlayableDirector GetTimeline(string id)
    {
        _timelineLookup.TryGetValue(id, out var director);
        return director;
    }

    public Flowchart GetFlowchart(string id)
    {
        _flowchartLookup.TryGetValue(id, out var flowchart);
        return flowchart;
    }
}