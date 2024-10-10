namespace TrophyLib
{
    public class TrophyRepository
    {
        private List<Trophy> trophies = new();
        private int nextId = 1;

        public IEnumerable<Trophy> Get(int? beforeYear = null, int? afterYear = null, string? sortBy = null)
        {
            IEnumerable<Trophy> results = new List<Trophy>(trophies);
            if(beforeYear != null)
            {
                results = results.Where(t => t.Year < beforeYear);
            }
            if(afterYear != null)
            {
                results = results.Where(t => t.Year > afterYear);
            }
            if(sortBy != null)
            {
                sortBy = sortBy.ToLower();
                switch(sortBy)
                {
                    case "id":
                    case "id_asc":
                        results = results.OrderBy(t => t.Id);
                        break;
                    case "id_desc":
                        results = results.OrderByDescending(t => t.Id);
                        break;
                    case "year":
                    case "year_asc":
                        results = results.OrderBy(t => t.Year);
                        break;
                    case "year_desc":
                        results = results.OrderByDescending(t => t.Year);
                        break;
                    case "competition":
                    case "competition_asc":
                        results = results.OrderBy(t => t.Competition);
                        break;
                    case "competition_desc":
                        results = results.OrderByDescending(t => t.Competition);
                        break;
                }
            }
            ICollection<Trophy> returnedResults = new List<Trophy>();
            foreach(Trophy trophy in results)
            {
                returnedResults.Add(new(trophy));
            }
            return returnedResults;
        }

        public Trophy? GetById(int id)
        {
            return trophies.FirstOrDefault(t => t.Id == id);
        }

        public Trophy Add(Trophy trophy)
        {
            if(trophy == null)
            {
                throw new ArgumentNullException($"{nameof(trophy)} must not be null");
            }
            trophy.Validate();
            trophy.Id = nextId++;
            trophies.Add(trophy);
            return trophy;
        }

        public Trophy? Remove(int id)
        {
            Trophy result = trophies.FirstOrDefault(t => t.Id == id);
            if(result != null)
            {
                trophies.Remove(result);
            }
            return result;
        }

        public Trophy? Update(int id, Trophy values)
        {
            if(values == null)
            {
                throw new ArgumentNullException();
            }
            values.Validate();

            Trophy result = trophies.FirstOrDefault(t => t.Id == id);
            if(result != null)
            {
                result.Competition = values.Competition;
                result.Year = values.Year;
            }
            return result;
        }
    }
}