using System.Linq;
using Event.Entity.Model;
using YooPoon.Core.Autofac;

namespace Event.Service.Participation
{
	public interface IParticipationService : IDependency
	{
		ParticipationEntity Create (ParticipationEntity entity);

		bool Delete(ParticipationEntity entity);

		ParticipationEntity Update (ParticipationEntity entity);

		ParticipationEntity GetParticipationById (int id);

		IQueryable<ParticipationEntity> GetParticipationsByCondition(ParticipationSearchCondition condition);

		int GetParticipationCount (ParticipationSearchCondition condition);

        int GetParticipationCountByCrowdId(int id);
	}
}