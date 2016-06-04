using System;

namespace askline {

    /// <summary>
    /// AgilePoint task model for future usage.
    /// </summary>
    public class AgilePointTask {

        public AgilePointTask(string workitem_id,
                              string task_name,
                              string proc_initor,
                              DateTime assigned_date,
                              string status,
                              string original_usr) {
            WorkitemId = workitem_id;
            TaskName = task_name;
            ProcessInitiator = proc_initor;
            AssignedDate = assigned_date;
            Status = status;
            OriginalUser = original_usr;
        }

        public string WorkitemId { get; }
        public string TaskName { get; }
        public string ProcessInitiator { get; }
        
        public DateTime AssignedDate { get; }
        public string Status { get; }
        public string OriginalUser { get; }
    }
}