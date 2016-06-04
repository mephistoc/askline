using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using askline.Models;

namespace askline {
    public class TaskConcierge {

        public IEnumerable<AgilePointTask> GetActiveTasks() {
            return new List<AgilePointTask>();
        }
    }
}