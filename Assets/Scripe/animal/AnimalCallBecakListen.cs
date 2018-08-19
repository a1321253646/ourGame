//动画的回调，如开始，结束（都仅能注册一个）,和帧监控，可以多个注册，但是相同帧
public class AnimalCallBecakListen
{
    public delegate void animalBegin(int status);
    public delegate void animalEnd(int status);
    public delegate void animalIndexCallback(int status);
    animalBegin begin;
    animalEnd end;
    public interface OnAnimalBeginCallbak {
        void animalBegin(int status);
    }
    public interface OnAnimalEndCallbak
    {
        void animalEnd(int status);
    }
    public interface OnAnimalIndexCallbak
    {
        void animalIndexCallback(int status);
    } 
}
