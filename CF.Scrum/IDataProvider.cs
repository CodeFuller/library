using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CF.Core;
using CF.Core.Exceptions;
using CF.Scrum.Objects;

namespace CF.Scrum
{
	public interface IDataProvider
	{
		IHistoricalDataProvider GetHistoricalDataProvider();

		Task<TObject> FindObjectByIdAsync<TObject>(string id, bool useCache) where TObject : BaseObject;

		Task<IEnumerable<Team>> GetTeamsAsync();
		Task<Team> GetTeamAsync(string teamName);

		Task<IEnumerable<Iteration>> GetTeamIterationsAsync(Team team);
		Task<Iteration> GetTeamIterationAsync(Team team, DateTime dateWithinIteration);
	}
}
