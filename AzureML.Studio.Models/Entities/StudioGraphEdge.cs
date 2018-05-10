﻿namespace AzureML.Studio.Models.Entities
{
    public class StudioGraphEdge
    {
        public string Id { get; set; }

        public StudioGraphNode SourceNode { get; set; }
        public StudioGraphNode DestinationNode { get; set; }
        public string UserData { get; set; }
    }
}
