using UnityEngine;

// for simplifying characterbody creation
public class BodyInfo
{
    public string bodyName = "";
    public string bodyNameToken = "";
    public string subtitleNameToken = "";

    public Texture characterPortrait = null;

    public GameObject crosshair = null;
    public GameObject podPrefab = null;

    public float maxHealth = 100f;
    public float healthGrowth = 2f;

    public float healthRegen = 0f;

    public float shield = 0f;// base shield is a thing apparently. neat
    public float shieldGrowth = 0f;

    public float moveSpeed = 7f;
    public float moveSpeedGrowth = 0f;

    public float acceleration = 80f;

    public float jumpPower = 15f;
    public float jumpPowerGrowth = 0f;// jump power per level exists for some reason

    public float damage = 12f;

    public float attackSpeed = 1f;
    public float attackSpeedGrowth = 0f;

    public float armor = 0f;
    public float armorGrowth = 0f;

    public float crit = 1f;
    public float critGrowth = 0f;

    public int jumpCount = 1;
}

// for simplifying rendererinfo creation
public class CustomRendererInfo
{
    public string childName;
    public Material material;
    public bool ignoreOverlays;
}