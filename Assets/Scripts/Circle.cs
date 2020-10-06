using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

[ExecuteInEditMode]
public class Circle : MonoBehaviour
{

    [Header("____Main Props____")]
    [SerializeField] Color color = Color.black;

    // Start is called before the first frame update
    void Start()
    {
    }

    SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer) return _spriteRenderer;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer;
        }
    }
    SpriteRenderer _spriteRenderer = null;

    void DestroySpriteIfNeeded()
    {
#if UNITY_EDITOR
        if (spriteRenderer.sprite != null)
            DestroyImmediate(spriteRenderer.sprite);
#else
        if(spriteRenderer.sprite != null)
            Destroy(spriteRenderer.sprite);
#endif
    }

    void BuildSprite()
    {
        // ベジェ曲線の情報を詰めるシェイプ定義
        var shape = new Shape();

        // シェイプの円作成に必要な情報を詰める
        var position = Vector2.zero;
        float radius = 100f;
        VectorUtils.MakeCircleShape(shape, position, radius);

        // 円の色を指定
        shape.Fill = new SolidFill()
        {
            //Color = Color.white
            Color = color
        };

        // 円のテッセレーションオプションを決める
        var options = new VectorUtils.TessellationOptions()
        {
            StepDistance = 10f,
            MaxCordDeviation = float.MaxValue,
            MaxTanAngleDeviation = Mathf.PI / 2.0f,
            SamplingStepSize = 0.01f
        };

        // ジオメトリの作成
        var node = new SceneNode()
        {
            Shapes = new List<Shape>
            {
                shape
            }
        };
        var scene = new Scene()
        {
            Root = node
        };
        var geoms = VectorUtils.TessellateScene(scene, options);

        // Spriteの作成と設定
        var sprite = VectorUtils.BuildSprite(
            geoms,
            100.0f,
            VectorUtils.Alignment.Center,
            Vector2.zero,
            128
            );

        spriteRenderer.sprite = sprite;


    }

    // Update is called once per frame
    void Update()
    {
        DestroySpriteIfNeeded();
        BuildSprite();
    }
}
