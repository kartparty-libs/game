using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingFade : MonoBehaviour
{
    // Start is called before the first frame update
    private float _alpha;
    private Image _img;
    void Start()
    {
        _img = GetComponent<Image>();
        _alpha = 0;
        _img.color = new Color(1, 1, 1, 0);
        StartCoroutine(show());
    }
    IEnumerator show()
    {
        while (_alpha < 1f)
        {
            _alpha += Time.deltaTime * 4f;
            _img.color = new Color(1, 1, 1, _alpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        while (_alpha > 0f)
        {
            _alpha -= Time.deltaTime * 2f;
            _img.color = new Color(1, 1, 1, _alpha);
            yield return null;
        }
        this.transform.parent.gameObject.SetActive(false);
    }
}
