using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;

public class Game : MonoBehaviour {
    [SerializeField] private GameObject block;
    [SerializeField] private TextAsset blocksDataJson;

    private const int BLOCKS_COUNT = 28;
    private readonly List<GameObject> blocks = new();
    private readonly BlockData[] blocksData = new BlockData[BLOCKS_COUNT];

    void Start() {
        for (int i = -4; i < 3; i++) {
            blocks.Add(Instantiate(block, new Vector3(-(i * 11 + 5.5f), 0.5f, -38.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(-38.5f, 0.5f, i * 11 + 5.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(i * 11 + 5.5f, 0.5f, 38.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(38.5f, 0.5f, -(i * 11 + 5.5f)), Quaternion.identity));
        }

        for (int i = 0; i < blocks.Count; i++)
            blocks[i].GetComponent<BlockController>().Number = (i % 4) * (BLOCKS_COUNT / 4) + i / 4;
        blocks.Sort((x, y) => x.GetComponent<BlockController>().Number.CompareTo(y.GetComponent<BlockController>().Number));
        foreach (var blockData in JsonSerializer.Deserialize<Dictionary<string, BlockData>>(blocksDataJson.text))
            blocksData[int.Parse(blockData.Key)] = blockData.Value;
        for (int i = 0; i < blocks.Count; i++) {
            BlockController bc = blocks[i].GetComponent<BlockController>();
            bc.Data = blocksData[i];
            bc.name = $"Block_{bc.Number,2:00}";
            bc.transform.SetParent(gameObject.transform);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A))
            GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), -90f, 2f);

        if (Input.GetKeyDown(KeyCode.D))
            GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), 90f, 2f);
    }
}
