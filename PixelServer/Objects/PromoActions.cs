namespace PixelServer.Objects;

///<summary>Promo Actions (top sellers and discounts)</summary>
public class PromoActions
{
    public List<string> topSellers_up { get; set; } = new();
    public List<string> discounts_up { get; set; } = new();

    public PromoActions()
    {
        topSellers_up = new()
        {
            "Assault_Machine_GunBuy",
            "Autoaim_RocketlauncherBuy",
            "Solar_Ray",
            "railgun",
            "RailRevolverBuy",
            "sword_of_shadows",
            "Red_Stone_3",
            "Solar_Power_Cannon",
            "toxic_bane",
            "Alien_Laser_Pistol",
            "DragonGun",
            "Trapper_1",
            "chainsaw_sword_1",
            "Charge_Cannon",
            "RayMinigun",
            "Antihero_Rifle_1",
            "cape_SkeletonLord",
            "hat_KingsCrown",
            "mask_sniper",
            "boots_black",
            "gadget_nucleargrenade",
            "gadget_resurrection",
            "gadget_jetpack",
            "gadget_Blizzard_generator",
            "gadget_fakebonus"
        };

        discounts_up = new();
    }
}