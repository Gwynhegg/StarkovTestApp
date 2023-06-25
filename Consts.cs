namespace StarkovTestApp
{
    public static class Consts
    {
        /// <summary>
        /// Перечисление возможных типов загружаемых таблиц
        /// </summary>
        public enum ContentTypes
        {
            Employee,
            Department,
            JobTitle
        }

        /// <summary>
        /// Список хранимых процедур, вызываемых программой
        /// </summary>
        public static class StoredProcedures
        {
            public const string UPDATE_INNER_DEP_IDS = "updateinnerdepid";
            public const string UPDATE_DEP_IDS = "updatedepartmentids";
            public const string UPDATE_JOB_TITLES = "updatejobtitles";
            public const string UPDATE_MANAGERS_IDS = "updatemanagerids";
        }
    }
}
