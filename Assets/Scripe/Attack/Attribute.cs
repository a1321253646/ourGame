public class Attribute
{
    public float aggressivity = 0;
    public float defense = 0;
    public float maxBloodVolume = 0;
    public float rate = 0;
    public float evd = 0;
    public float crt = 0;
    public float hurt = 0;
    public float crtHurt = 0;
    public float readHurt = 0;
    public float attackSpeed = 1;

    public Attribute add(Attribute adder) {
        aggressivity += adder.aggressivity;
        defense += adder.defense;
        rate += adder.rate;
        evd += adder.aggressivity;
        maxBloodVolume += adder.maxBloodVolume;
        crt += adder.crt;
        hurt += adder.hurt;
        crtHurt += adder.crtHurt;
        readHurt += adder.readHurt;
        attackSpeed += adder.attackSpeed;
        return this;
    }
    public void clear()
    {
        aggressivity = 0;
        defense = 0;
        rate = 0;
        evd = 0;
        maxBloodVolume = 0;
        crt = 0;
        hurt = 0;
        crtHurt = 0;
        readHurt = 0;
        attackSpeed = 1;
    }
    public string toString() {
        string s = "";
        s += ("aggressivity=" + aggressivity + "\n");
        s += ("defense=" + defense + "\n");
        s += ("maxBloodVolume=" + maxBloodVolume + "\n");
        s += ("rate=" + rate + "\n");
        s += ("evd=" + evd + "\n");
        s += ("crt=" + crt + "\n");
        s += ("hurt=" + hurt + "\n");
        s += ("crtHurt=" + crtHurt + "\n");
        s += ("readHurt=" + readHurt + "\n");
        s += ("attackSpeed=" + attackSpeed +"\n");
        return s;
    }
}
