using System.Collections.Generic;

namespace BossRush2;

public class Team
{
	public List<Entity> members = [];
	public string name;
	public int collisionLayer;
	public List<Team> collisionMask = [];
	public void AddMember(Entity member)
	{
		members.Add(member);
	}
	public void RemoveMember(Entity member)
	{
		members.Remove(member);
	}
}
