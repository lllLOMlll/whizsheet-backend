namespace Whizsheet.Api.Dtos.Characters
{
	public class CharacterDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Class { get; set; } = string.Empty;
		public int Hp {  get; set; }
	}
}
