using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem.Handlers;


public enum ModelStateEnum
{
    Fresh,
    Annotated
}
public class ModelPiece : MonoBehaviour, IMixedRealityPointerHandler
{

    int _modelState = 0;
    public int ModelState
    {
        get
        {
            return _modelState;
        }
        set
        {
            _modelState = value;
            UpdateModelState((ModelStateEnum)_modelState);
        }
    }

    //bool _isOpened = false;
    string _annotationText = "";
    public string AnnotationText {
        get { return _annotationText; }
        set {
            _annotationText = value;
            annotationTextMesh.text = _annotationText;
            Debug.Log("Annotation value is " + _annotationText);
        }
    }
    string _translationText = "";
    public string TranslationText
    {
        get { return _translationText; }
        set
        {
            _translationText = value;
            translationTextMesh.text = _translationText;
        }
    }
    private Vector3 initLocalPostion;


    [SerializeField] private MySpatialButton     buttonUIReference;
    [SerializeField] private GameObject annotationUIReference;
    [SerializeField] private TextMesh translationTextMesh;
    [SerializeField] private TextMesh annotationTextMesh;

    private void Awake()
    {
        initLocalPostion = this.transform.localPosition;
        annotationTextMesh.text = "";
        translationTextMesh.text = "";
    }

    private void UpdateModelState(ModelStateEnum newState)
    {

    }


    public void OpenModel()
    {
        //Apply fly out animation.
        var playerPosition = Camera.main.transform.position;
        var modelPositionOffset = Camera.main.transform.forward * 0.5f;
        var positionToShow = transform.parent.InverseTransformPoint(playerPosition + modelPositionOffset);
        StartCoroutine(AnimationUtility.LocalMoveAnimation(gameObject, 0.5f, transform.localPosition, positionToShow));

        showUI();
        GetComponent<Collider>().enabled = false;
    }

    public void CloseModel()
    {
        //Apply fly back animation.
        StartCoroutine(AnimationUtility.LocalMoveAnimation(gameObject, 0.5f, transform.localPosition, initLocalPostion));

        GetComponent<Collider>().enabled = true;
    }

    private void showUI()
    {
        if ((ModelStateEnum)ModelState == ModelStateEnum.Fresh)
        {
            showFreshUI();
        }
        else if((ModelStateEnum)ModelState == ModelStateEnum.Annotated)
        {
            showAnnotatedUI();
        }
    }

    private void showFreshUI()
    {
        buttonUIReference.ButtonState = (int)MySpatialButtonState.RecordFront;
    }
    private void showAnnotatedUI()
    {
        annotationUIReference.transform.localScale = Vector3.one;
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        OpenModel();
        eventData.Use();
        //if (!_isOpened)
        //{
        //    OpenModel();
        //    _isOpened = true;
        //}
        //else
        //{
        //    CloseModel();
        //    _isOpened = false;
        //}
    }
}
