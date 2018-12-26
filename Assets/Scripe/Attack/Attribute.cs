public class Attribute
{
    public double aggressivity = 0;
    public double defense = 0;
    public double maxBloodVolume = 0;
    public float rate = 0;
    public float evd = 0;
    public float crt = 0;
    public float hurt = 0;
    public double crtHurt = 0;
    public double readHurt = 0;
    public float attackSpeed = 0;

    public Attribute add(Attribute adder) {
        aggressivity += adder.aggressivity;
        defense += adder.defense;
        rate += adder.rate;
        evd += adder.evd;
        maxBloodVolume += adder.maxBloodVolume;
        crt += adder.crt;
        hurt += adder.hurt;
        crtHurt += adder.crtHurt;
        readHurt += adder.readHurt;
        attackSpeed += adder.attackSpeed;
        return this;
    }
    public Attribute chen(Attribute adder)
    {
        aggressivity = chenGetDouble(aggressivity, adder.aggressivity / 10000) ;
        defense = chenGetDouble(defense, adder.defense / 10000);
        rate = (float)chenGetInt(rate, adder.rate / 10000);
        evd = (float)chenGetInt(evd, adder.evd / 10000);
        maxBloodVolume = chenGetDouble(maxBloodVolume, adder.maxBloodVolume / 10000);
        crt = (float)chenGetInt(crt, adder.crt / 10000);
        hurt = (float)chenGetInt(hurt, adder.hurt / 10000);
        crtHurt = chenGetDouble(crtHurt, adder.crtHurt / 10000);
        readHurt = chenGetDouble(readHurt, adder.readHurt / 10000);
        attackSpeed = (float)chenGetInt(attackSpeed, adder.attackSpeed / 10000);
        return this;
    }

    private double chenGetInt(double a1, double a2) {
        if (a2 < 0) {
            a2 = 0;
        }
        double a3 = a1 * a2;
        return a3;
    }
    private double chenGetDouble(double a1, double a2)
    {
        double a3 = a1 * a2;
        int tmp = a3 % 1 == 0 ? 0 : 1;
        a3 = a3 / 1 + tmp;
        return a3;
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
        attackSpeed = 0;
    }
    public Attribute setToPre() {
        aggressivity = 10000;
        defense = 10000;
        rate = 10000;
        evd = 10000;
        maxBloodVolume = 10000;
        crt = 10000;
        hurt = 10000;
        crtHurt = 10000;
        readHurt = 10000;
        attackSpeed = 10000;
        return this;
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
