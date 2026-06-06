namespace Rpg.Player.Service.Dtos
{
    public record LevelUpResultDto
    (
        int PreviousLevel,
        int Nextlevel,

        int HpGained,
        int MpGained,

        int StrengthGained,
        int DefenseGained,

        int AgilityGained,
        int WisdomGained,

        int LuckGained

    );
}
