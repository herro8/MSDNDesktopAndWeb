namespace Ahead.Data
{
    public class OperationModel
    {
        public string Result { get; set; }

        public string Message { get; set; }

        public string ReturnValue { get; set; }
    }
    
    public class TeamMember
    {
        public string Alias { get; set; }
        public string ChineseName { get; set; }
        public double OverTime { get; set; }
        public double Annual { get; set; }
        public string Team { get; set; }
    }

   
}