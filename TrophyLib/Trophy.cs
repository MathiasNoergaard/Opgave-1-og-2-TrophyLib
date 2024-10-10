namespace TrophyLib
{
    public class Trophy
    {
        public int Id { get; set; }
        public string Competition { get; set; }
        public int Year { get; set; }

        public Trophy()
        {
        }

        public Trophy(Trophy copyMyValues)
        {
            if(copyMyValues == null)
            {
                throw new ArgumentNullException($"{nameof(copyMyValues)} must not be null");
            }
            copyMyValues.Validate();

            Id = copyMyValues.Id;
            Competition = copyMyValues.Competition;
            Year = copyMyValues.Year;
        }


        public void ValidateCompetition()
        {
            if(Competition == null)
            {
                throw new ArgumentNullException($"{nameof(Competition)} must not be null");
            }
            if(Competition.Length < 3)
            {
                throw new ArgumentException($"{nameof(Competition)}, {Competition.Length} characters, must be atleast 3 characters long");
            }
        }

        public void ValidateYear()
        {
            if(Year < 1970)
            {
                throw new ArgumentOutOfRangeException($"{nameof(Year)}, {Year}, must be 1970 higher");
            }
            if(Year > 2024)
            {
                throw new ArgumentOutOfRangeException($"{nameof(Year)}, {Year}, must be less than or equal to 2024");
            }
        }

        public void Validate()
        {
            ValidateCompetition();
            ValidateYear();
        }

        public override string ToString()
        {
            return $"{Id} {Competition} {Year}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Trophy trophy &&
                   Id == trophy.Id &&
                   Competition == trophy.Competition &&
                   Year == trophy.Year;
        }
    }
}