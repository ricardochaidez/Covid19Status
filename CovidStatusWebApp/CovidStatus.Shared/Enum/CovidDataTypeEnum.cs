namespace CovidStatus.Shared.Enum
{
    public enum CovidDataTypeEnum : byte
    {
        NewCases = 1,
        NewDeaths = 2,
        ICUCovidPatients = 3,
        CasesMovingAverage = 4,
        DeathsMovingAverage = 5,
        CasesMovingAveragePerOneThousandPopulation = 6,
        DeathsMovingAveragePerOneThousandPopulation = 7
    }
}
