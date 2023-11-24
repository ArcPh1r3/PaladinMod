using RoR2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditoRecordAndSetTransforms {

    public static List<Transform> _transforms;

    public static Vector3[] _storedPositions;
    public static Quaternion[] _storedRotations;
    public static Vector3[] _storedScales;

    public static Collider storedCollider;
    public static Rigidbody storedRigidbody;
    public static CharacterJoint storedJoint;
    public static ChildLocator storedChildLocator;
    public static RagdollController storedRagdollController;

    [MenuItem("CONTEXT/Transform/Record - Record All Child Transforms")]
    public static void getAllTransformPositions() {

        FillTransformList();

        _storedPositions = new Vector3[_transforms.Count];
        _storedRotations = new Quaternion[_transforms.Count];
        _storedScales = new Vector3[_transforms.Count];

        int aye = 0;
        for (int i = 0; i < _transforms.Count; i++) {

            if (_transforms[i]) {
                _storedPositions[i] = _transforms[i].position;
                _storedRotations[i] = _transforms[i].rotation;
                _storedScales[i] = _transforms[i].localScale;

                aye++;
            }
        }

        Debug.Log($"{aye}/{_transforms.Count} transforms have been recorded. Turn off animation Preview and click 'Set ALL Recorded Transforms'");
    }

    private static void FillTransformList() {

        if (_transforms == null)
            _transforms = new List<Transform>();

        Transform[] children;
        for (int select = 0; select < Selection.transforms.Length; select++) {

            //NEVER do GetComponentsInChildren at runtime, unless you don't care about high FPS in which case you're not a true gamer -ts
            children = Selection.transforms[select].GetComponentsInChildren<Transform>();

            //holy shit i'm doing the SRM thing kinda
            for (int i = 0; i < children.Length || i < _transforms.Count; i++) {

                if (i < children.Length) {

                    if (_transforms.Count <= i) {
                        _transforms.Add(children[i]);
                    } else {
                        _transforms[i] = children[i];
                    }
                } else {
                    _transforms[i] = null;
                }
            }
        }
    }

    [CanEditMultipleObjects]
    [MenuItem("CONTEXT/Transform/Record - Set All Recorded Transforms")]
    public static void setAllTransformPositions() {

        if (_transforms == null) {
            Debug.LogError("no transforms are recorded. use Record Transforms first");
            return;
        }

        Undo.RecordObjects(_transforms.ToArray(), "setting transforms");

        for (int i = 0; i < _transforms.Count; i++) {

            if (_transforms[i] != null) {
                _transforms[i].position = _storedPositions[i];
                _transforms[i].rotation = _storedRotations[i];
                _transforms[i].localScale = _storedScales[i];
            }
        }
    }
}


public class EditoRecordAndSetTransformsAbsolute {
    public class StoredTransformInfo {
        public string transformName;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;
    }

    public static List<StoredTransformInfo> _storedTransformInfos;

    [MenuItem("CONTEXT/Transform/Record Absolute - Record All Child Transform Positions")]
    public static void getAllTransformPositions() {

        if (_storedTransformInfos == null)
            _storedTransformInfos = new List<StoredTransformInfo>();

        _storedTransformInfos.Clear();

        Transform[] children;
        Transform child;
        for (int select = 0; select < Selection.transforms.Length; select++) {

            children = Selection.transforms[select].GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++) {
                child = children[i];
                _storedTransformInfos.Add(new StoredTransformInfo {
                    transformName = child.name,
                    position = child.position,
                    rotation = child.rotation,
                    localScale = child.localScale
                });
            }
        }

        Debug.Log(_storedTransformInfos.Count + " transforms recorded");
    }

    [CanEditMultipleObjects]
    [MenuItem("CONTEXT/Transform/Record Absolute - Set Child Transforms To Recorded Positions")]
    public static void setAllTransformPositions() {

        if (_storedTransformInfos == null) {
            Debug.LogError("no transforms are recorded. Record Transforms first");
            return;
        }

        Transform[] children;
        Transform selectedChild;
        StoredTransformInfo foundInfo;
        for (int select = 0; select < Selection.transforms.Length; select++) {

            children = Selection.transforms[select].GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++) {
                selectedChild = children[i];
                Undo.RecordObject(selectedChild, "set transforms absolute");
                foundInfo = _storedTransformInfos.Find((info) => info.transformName == selectedChild.name);
                if (foundInfo != null) {
                    selectedChild.position = foundInfo.position;
                    selectedChild.rotation = foundInfo.rotation;
                    selectedChild.localScale = foundInfo.localScale;
                }
            }
        }
    }
}
