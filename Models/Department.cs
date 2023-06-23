﻿namespace StarkovTestApp.Models
{
    public class Department
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public int ManagerID { get; set; }
        public string ManagerName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool NotValid { get; set; }
    }
}
