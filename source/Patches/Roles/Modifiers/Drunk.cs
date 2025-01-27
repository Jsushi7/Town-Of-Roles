namespace TownOfSushi.Roles.Modifiers
{
    public class Drunk : Modifier
    {
        public Drunk(PlayerControl player) : base(player)
        {
            Name = "Drunk";
            TaskText = () => "Your controls are inverted!";
            Color = Patches.Colors.Drunk;
            ModifierType = ModifierEnum.Drunk;
        }
    }
}