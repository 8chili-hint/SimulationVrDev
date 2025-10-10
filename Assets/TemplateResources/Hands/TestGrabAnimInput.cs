using UnityEngine;

public class TestGrabAnimInput : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private bool isleft = false;
    private const string _right = "right", _left = "left";
    private void OnEnable()
    {
        _anim.SetBool(_left, isleft);
        _anim.SetBool(_right, !isleft);
    }
    
    //// Update is called once per frame
    //void Update()
    //{
    //    if(Input.GetMouseButton(1))
    //    {
    //        _anim.SetBool(_left, false);
    //        _anim.SetBool(_right, true);
    //    }

    //    if(Input.GetMouseButton(0))
    //    {
    //        _anim.SetBool(_right, false);
    //        _anim.SetBool(_left, true);
    //    }
    //}
}
