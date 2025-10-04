using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Cooking : MonoBehaviour
{
    [Header("Beef Cooking")]
    [SerializeField] private AnimaLeaf animaLeaf;
    [SerializeField] private AnimBeef animaBeff;
    [SerializeField] private AnimPepper animaPepper;
    [SerializeField] private AnimBeefHT animBeefHT;
    [SerializeField] private Animator animator;
    [SerializeField] DragBeefChicken dragChicken;
    [SerializeField] private CountOBJ countOBJ;
    [SerializeField] public GameObject[] ingredients; // 3 nguyên liệu bỏ vô nồi
    [SerializeField] private GameObject finalDish;     // prefab hoặc object món hoàn thành (có SpriteRenderer)
    [SerializeField] private GameObject waterTrue;

    [Header("Chicken Cooking")]
    [SerializeField] private GameObject chicken;        // object thịt gà
    [SerializeField] private GameObject botMo; 

    private bool isCooking = false;
    private bool isCookingChicken = false;
    private Vector3 originalPos;
    private Vector3 originalScale;

    public static bool CookingFinished = false;

    private void Update()
    {
        if (countOBJ.obIndex == 3 && !isCooking)
        {
            StartCooking();
        }
        // Cook chicken riêng
        if (chicken != null && chicken.activeSelf && !isCookingChicken && dragChicken.isClayPot == true)
        {
            StartCoroutine(CookChicken());
        }
    }
    private void StartCooking()
    {
        isCooking = true;

        // Hiệu ứng "sôi nước" nhẹ nhàng trong 3 giây
        originalPos = transform.localPosition;
        originalScale = transform.localScale;

        // Hiệu ứng "sôi nước" nhẹ nhàng trong 3 giây
        Sequence boilingSeq = DOTween.Sequence();

        // Lắc nhẹ qua lại quanh vị trí gốc
        boilingSeq.Append(transform.DOLocalMoveX(originalPos.x + 0.05f, 0.4f).SetEase(Ease.InOutSine));
        boilingSeq.Append(transform.DOLocalMoveX(originalPos.x - 0.05f, 0.4f).SetEase(Ease.InOutSine));
        boilingSeq.SetLoops(-1, LoopType.Yoyo);

        // Thêm hiệu ứng phồng nhẹ Y quanh scale gốc
        transform.DOScaleY(originalScale.y * 1.05f, 0.8f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);

        
        animaBeff.OnAnimBeef();
        animaLeaf.OnAnimLeaf();
        animaPepper.OnAnimPepper();
        animator.SetTrigger("WaterFalse");


        finalDish.SetActive(true);
        animBeefHT.OnAnimBeefHT();
        waterTrue.SetActive(true);


        // Sau 3 giây thì nấu xong
        DOVirtual.DelayedCall(5f, () =>
        {
            animBeefHT.AnimatorFalse();
            // Dừng hiệu ứng sôi
            boilingSeq.Kill();
            transform.DOKill();

            // Reset về vị trí gốc
            transform.localScale = originalScale;
            transform.localPosition = originalPos;

            // // Ẩn dần nguyên liệu
            // foreach (var ing in ingredients)
            // {
            //     if (ing != null)
            //     {
            //         // dừng hẳn hiệu ứng nổi
            //         var dropObj = ing.GetComponent<DrapDropOBJ>();
            //         if (dropObj != null) dropObj.StopFloating();


            //         ing.transform.DOScale(Vector3.zero, 0.5f)
            //             .SetEase(Ease.InBack)
            //             .OnComplete(() => ing.SetActive(false));
            //     }
            // }


            // Đánh dấu đã nấu xong
            CookingFinished = true;


            // // Hiện món hoàn thành
            // finalDish.SetActive(true);
            // var sr = finalDish.GetComponent<SpriteRenderer>();
            // sr.color = new Color(1, 1, 1, 0); // ban đầu trong suốt
            // finalDish.transform.localScale = Vector3.zero; // ban đầu nhỏ

            // // Animate lớn dần + fade in
            // finalDish.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.6f)
            //     .SetEase(Ease.OutBack);
            // sr.DOFade(1f, 0.6f).SetEase(Ease.InOutSine);
        });
    }
    private IEnumerator CookChicken()
    {
        isCookingChicken = true;

        originalPos = transform.localPosition;
        originalScale = transform.localScale;

        // Hiệu ứng sôi
        Sequence boilingSeq = DOTween.Sequence();
        boilingSeq.Append(transform.DOLocalMoveY(originalPos.y + 0.05f, 0.4f).SetEase(Ease.InOutSine));
        boilingSeq.Append(transform.DOLocalMoveY(originalPos.y - 0.05f, 0.4f).SetEase(Ease.InOutSine));
        boilingSeq.SetLoops(-1, LoopType.Yoyo);

        transform.DOScaleX(originalScale.x * 1.05f, 0.8f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);

        // Chờ 5s
        yield return new WaitForSeconds(3f);

        botMo.SetActive(true);

        yield return new WaitForSeconds(7f);

        // Stop boiling
        boilingSeq.Kill();
        transform.DOKill();

        transform.localScale = originalScale;
        transform.localPosition = originalPos;

        // // Ẩn chicken
        // if (chicken != null)
        // {
        //     chicken.transform.DOScale(Vector3.zero, 0.5f)
        //         .OnComplete(() => chicken.SetActive(false));
        // }

        // 👉 Chuyển màn khác (bạn đã có code rồi)
        // Ví dụ: SceneManager.LoadScene("NextScene");
        Debug.Log("Chicken cooked! Go to next scene.");
    }
}
