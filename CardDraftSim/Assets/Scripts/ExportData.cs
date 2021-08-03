using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportData : MonoBehaviour
{

    public class DraftData
    {
        public int draftIndex;
        public int commonCardsInDraft;
        public int uncommonCardsInDraft;
        public int rareCardsInDraft;

        public DraftData(int index, int common, int uncommon, int rare)
        {
            draftIndex = index;
            commonCardsInDraft = common;
            uncommonCardsInDraft = uncommon;
            rareCardsInDraft = rare;
        }
    }

    public class DraftList
    {
        public List<DraftData> draftData = new List<DraftData>();
    }

    DraftList draftList = new DraftList();
    CardDraft cardDraft;
    string filename = "";

    [SerializeField] int minDraftsForExport = 6;

    private void Awake()
    {
        cardDraft = FindObjectOfType<CardDraft>();
        cardDraft.sendData += RetreiveData;

        filename = Application.dataPath + "/draftdata.csv";
    }

    private void RetreiveData()
    {
        DraftData data = new DraftData(cardDraft.DraftIndex, cardDraft.CommonCardsInDraft, cardDraft.UncommonCardsInDraft, cardDraft.RareCardsInDraft);
        draftList.draftData.Add(data);
    }

    public void ExportDataToFile()
    {
        if (draftList.draftData.Count < minDraftsForExport)
        {
            Debug.Log(string.Format("Insufficient drafts for export. Min drafts required: {0}", minDraftsForExport));
            return;
        }

        TextWriter textWriter = new StreamWriter(filename, false);
        textWriter.WriteLine("Index,Common,Uncommon,Rare");
        textWriter.Close();

        textWriter = new StreamWriter(filename, true);
        for (int i = 0; i < draftList.draftData.Count; i++)
        {
            if (draftList.draftData[i].draftIndex == 1)
            {
                textWriter.WriteLine("X,X,X,X");
            }

            textWriter.WriteLine(string.Format("{0},{1},{2},{3}", draftList.draftData[i].draftIndex,
                                                                  draftList.draftData[i].commonCardsInDraft,
                                                                  draftList.draftData[i].uncommonCardsInDraft,
                                                                  draftList.draftData[i].rareCardsInDraft));
        }
        textWriter.Close();
        Debug.Log("Data Exported");
    }    
}
