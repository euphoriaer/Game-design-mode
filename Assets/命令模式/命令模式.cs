using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class 命令模式 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DoBtn_Lambda;

    public GameObject DoBtn_Class;

    public GameObject DoLamObj;
    public GameObject DoClassObj;

    public Vector3 Direction;
    public float speed;

    public Command cmdClass;

    private void Start()
    {
        DoBtn_Lambda.GetComponent<Button>().onClick.AddListener(CmdDo);
        DoBtn_Class.GetComponent<Button>().onClick.AddListener(CmdClass);
    }

    private void CmdClass()
    {
        cmd = new Move(DoClassObj, Direction, speed);
        var cmd2 = cmd.Do((() => Debug.Log("A")));
        var cmd3 = cmd2.Do((() => Debug.Log("B")));
        var cmd4 = cmd3.Do((() => Debug.Log("C")));
        cmdClass = cmd4;
        //适合带撤回，做一个命令栈，往回执行undo即可
    }

    private Move cmd;

    private void CmdDo()
    {
        cmd = new Move(DoLamObj, Direction, speed);

        cmd.Do((() =>
        {
            Debug.Log("1");
        })).Do((() =>
        {
            Debug.Log(2);
        })).Do((() => Debug.Log("3"))).Uo();
    }

    //lambda 闭包，适合流式编程，不适合带撤回，因为匿名方法没办法undo
}

public abstract class Command
{
    public abstract Command Do(UnityAction action);

    public abstract void Uo();
}

public class Move : Command
{
    private GameObject _obj;
    private Vector3 _movedirection;
    private float _speed;

    private Vector3 _beforePositon;

    public Move(GameObject obj, Vector3 direction, float speed)
    {
        this._obj = obj;
        this._movedirection = direction;
        this._speed = speed;
        _beforePositon = _obj.transform.position;
    }

    public override Command Do(UnityAction action)
    {
        _obj.transform.Translate(_movedirection * _speed, Space.World);
        action();
        return new Move(_obj, _movedirection, _speed);
    }

    public override void Uo()
    {
        if (_beforePositon != null)
        {
            _obj.transform.position = _beforePositon;
        }
    }
}