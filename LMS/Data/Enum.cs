namespace LMS.Data.Enum
{
    public enum Role
    {
        STUDENT = 1,
        PROFESSOR = 2,
        ADMIN = 3
    }

    public enum AssignmentType
    {
        ASSIGNMENT = 1,
        QUIZ = 2,
        TEST = 3,
        EXAM = 4,
        SURVEY = 5
    }

    public enum SubmissionType
    {
        TEXT = 1,
        FILE_UPLOAD = 2,
    }

    public enum NotificationType
    {
        SYSTEM = 1,
        ANNOUNCEMENT = 2,
        EMAIL = 3
    }

    /// <summary>
    /// Standard folders registered with the system. These folders are installed with Windows Vista
    /// and later operating systems, and a computer will have only folders appropriate to it
    /// installed.
    /// </summary>
    public enum KnownFolder
    {
        Contacts,
        Desktop,
        Documents,
        Downloads,
        Favorites,
        Links,
        Music,
        Pictures,
        SavedGames,
        SavedSearches,
        Videos
    }
}
