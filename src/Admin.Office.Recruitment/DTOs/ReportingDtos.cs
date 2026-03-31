namespace Admin.Office.Recruitment.DTOs;

public record RecruitmentStatsDto(
    int TotalApplicants,
    int OpenPositions,
    int NewApplications,
    int ToRecruit
);

public record RecruitmentTrendDto(
    string Month,
    Dictionary<string, int> PositionCounts
);
