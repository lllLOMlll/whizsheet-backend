using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain
{
	public class Skill
	{
		public int Id { get; set; }

		public SkillType Type { get; private set; }

		public bool IsProficient { get; private set; }

		public int CharacterId { get; private set; }
		public Character Character { get; private set; } = null!;


		private Skill() { }

		public Skill(SkillType type)
		{
			Type = type;
			IsProficient = false;
		}

		public void SetProficiency(bool value)
		{
			IsProficient = value;
		}
	}
}
