// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class DynamicContent : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject panelPrefab;
        [SerializeField] private Toggle togglePrefab;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private SimpleScrollSnap scrollSnap;
        [SerializeField] private Texture2D[] _sprites;
        [SerializeField] private PuzzleConfig _config;

        private float toggleWidth;
        #endregion

        private void Awake()
        {
            toggleWidth = (togglePrefab.transform as RectTransform).sizeDelta.x * (Screen.width / 2048f); ;
        }

        private void Start()
        {
            foreach (var s in _sprites)
            {
                StartCoroutine(Add(0, s));
            }
        }
        
        #region PublicMethods


        private IEnumerator Add(int index, Texture2D texture)
        {
            yield return new WaitForEndOfFrame();

            var obj = panelPrefab;
            obj.name = texture.name;
            var image = obj.GetComponent<RawImage>();
            image.texture = texture;
            scrollSnap.Add(obj, index);
        }

        public void SetImage()
        {
            var child = scrollSnap.Content.transform.GetChild(scrollSnap.SelectedPanel);
            _config.image = (Texture2D) child.GetComponent<RawImage>().mainTexture;
            //_config.image = TextureStruct.convertSpriteToTexture2D(child.GetComponent<Image>().sprite);
            Debug.Log("asdasdas");
        }

        #endregion
    }
}