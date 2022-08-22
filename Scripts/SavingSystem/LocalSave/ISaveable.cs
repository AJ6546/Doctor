// This Interface is inheritted by each script that needs saving someething.
public interface ISaveable
{
   object CaptureState();
   void RestoreState(object state);
}