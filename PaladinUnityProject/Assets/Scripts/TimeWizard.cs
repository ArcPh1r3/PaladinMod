using UnityEngine;

public class TimeWizard : MonoBehaviour {

    private void Update() {
        Tim();
    }

    private void Tim() {
        //time keys
        if (Input.GetKeyDown(KeyCode.I)) {
            if (Time.timeScale == 0) {
                setTimeScale(Time.timeScale + 0.1f);
            } else {
                setTimeScale(Time.timeScale + 0.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.K)) {

            setTimeScale(Time.timeScale - 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            setTimeScale(1);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            setTimeScale(0);
        }
    }

    private void setTimeScale(float tim) {
        Time.timeScale = tim;

        Debug.Log($"set tim: {Time.timeScale}");
    }
}
