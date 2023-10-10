using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameManager : MonoBehaviour {
    public static GameObject selectedObject;
	public Material[] Materials = new Material[10];
	public GameObject[] Blocks = new GameObject[9];
    public static bool selected = false;
	public static string text;
	public static int[,] t = new int[3, 3] {
    { 0, 0, 0},
    { 0, 0, 0},
    { 0, 0, 0}
    };
    public static int[,] r = new int[3, 3] {
    { 0, 0, 0},
    { 0, 0, 0},
    { 0, 0, 0}
    };

    public void Solve()
    {
        int[,,] v;
        List<int[,,]> all = new();
		List<int[,,]> vReverses;
        List<List<int[,,]>> allReverse = new();
        List<int> prevDir = new();
        List<string> path = new();
        List<List<int>> prevDirReverse = new();
        List<List<string>> pathReverse = new();

        v = CreateV(t, r);
        if (!Possible(v, t))
        {
			text = "This setup is not possible!";
            return;
        }
        if (Finished(v))
		{
			text = "This is already finished!";
			return;
		}
		all.Add(new int[3, 3, 4]);
		Array.Copy(v, all[^1], 36);
		prevDir.Add(4);
        path.Add("");
        vReverses = FinishPositions(v);
        for (int i = 0; i < vReverses.Count; i++)
        {
            allReverse.Add(new());
            allReverse[i].Add(new int[3, 3, 4]);
            allReverse[i][0] = vReverses[i];
            pathReverse.Add(new());
            pathReverse[i].Add("");
            prevDirReverse.Add(new());
            prevDirReverse[i].Add(5);
        }
        int count = 0;
        while (true)
        {
            count++;
			if(count == 32)
			{
				text = "This setup is not possible!";
				return;
			}
			if (count % 2 == 1)
			{
				AddMove(all, prevDir, path);
			}
			else
			{
				for (int i = 0; i < allReverse.Count; i++)
				{
					AddMove(allReverse[i], prevDirReverse[i], pathReverse[i]);
				}
			}
            for (int i = 0; i < allReverse.Count; i++)
            {
                for (int j = 0; j < allReverse[i].Count; j++)
                {
                    for (int k = 0; k < all.Count; k++)
                    {
                        if (EqualV(all[k], allReverse[i][j]))
                        {
							text = "Solution: " + path[k] + FixPath(pathReverse[i][j]) + "\nMoves: " + (path[k].Count() + pathReverse[i][j].Count()) / 2;
							return;
                        }
                    }
                }
            }
        }
    }

    int[,,] CreateV(int[,] t, int[,] r)
    {
        int[,,] v = new int[3, 3, 4];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					v[i, j, k] = 0;
				}
				switch (t[i, j])
				{
					case 0:
						for (int k = 0; k < 4; k++)
						{
							v[i, j, k] = -1;
						}
						break;
					case 1:
						for (int k = 0; k < 4; k++)
						{
							v[i, j, k] = -2;
						}
						break;
					case 2:
						v[i, j, 0] = 1;
						break;
					case 3:
						v[i, j, 0] = 1;
						v[i, j, 2] = 1;
						break;
					case 4:
						v[i, j, 0] = 2;
						v[i, j, 1] = 2;
						break;
					case 5:
						v[i, j, 0] = 1;
						v[i, j, 3] = 1;
						break;
					case 6:
						v[i, j, 2] = 2;
						break;
					case 7:
						v[i, j, 1] = 1;
						v[i, j, 2] = 2;
						break;
					case 8:
						v[i, j, 2] = 2;
						v[i, j, 3] = 1;
						break;
					case 9:
						v[i, j, 1] = 2;
						v[i, j, 3] = 2;
						break;
				}
				for (int k = 0; k < r[i, j] % 4; k++)
				{
					int temp = v[i, j, 0];
					v[i, j, 0] = v[i, j, 3];
					v[i, j, 3] = v[i, j, 2];
					v[i, j, 2] = v[i, j, 1];
					v[i, j, 1] = temp;
				}
			}
		}

		return v;
	}

	bool Possible(int[,,] v, int[,] t)
	{
		int count;
		for (int k = 0; k < 10; k++)
		{
			count = 0;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (t[i, j] == k)
					{
						count++;
					}
				}
			}
			if ((k == 0 && count != 1) || ((k == 2 || k == 3 || k == 4 || k == 5 || k == 6 || k == 7 || k == 8 || k == 9) && count >= 2))
			{
				return false;
			}
		}
		int countLegs, countHeads;
		for (int k = 0; k < 4; k++)
		{
			countLegs = 0;
			countHeads = 0;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (v[i, j, k] == 1)
					{
						countLegs++;
					}
					else if (v[i, j, (k + 2) % 4] == 2)
					{
						countHeads++;
					}
				}
			}
			if (countLegs != countHeads)
			{
				return false;
			}
		}
		return true;
	}

	bool Finished(int[,,] v)
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if ((i == 0 && v[0, j, 0] > 0) || (i == 2 && v[2, j, 2] > 0) || (j == 0 && v[i, 0, 3] > 0) || (j == 2 && v[i, 2, 1] > 0))
				{
					return false;
				}
				if ((v[i, j, 0] > 0 && v[i - 1, j, 2] + v[i, j, 0] != 3) ||
				   (v[i, j, 1] > 0 && v[i, j + 1, 3] + v[i, j, 1] != 3) ||
				   (v[i, j, 2] > 0 && v[i + 1, j, 0] + v[i, j, 2] != 3) ||
				   (v[i, j, 3] > 0 && v[i, j - 1, 1] + v[i, j, 3] != 3))
				{
					return false;
				}
			}
		}
		return true;
	}

	void DebugV(int[,,] v)
	{
		string line = "";
		for (int i = 0; i < 3; i++)
		{
			for (int k = 0; k < 3; k++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (k == 0)
					{
						line += "  " + v[i, j, 0] + "  " + (v[i, j, 0] < 0 ? "" : "  ");
					}
					else if (k == 1)
					{
						line += v[i, j, 3] + (v[i, j, 3] < 0 ? "" : "  ") + v[i, j, 1] + (v[i, j, 1] < 0 ? "" : "  ");
					}
					else
					{
						line += "  " + v[i, j, 2] + "  " + (v[i, j, 2] < 0 ? "" : "  ");
					}
				}
                line += "\n";
            }
		}
		Debug.Log(line);
	}

	List<int[,,]> FinishPositions(int[,,] v)
	{
		List< int[,,]> vReverses = new ();
		int[,,] vReverse = new int[3, 3, 4];
		bool breakk;
		for (int a = 0; a < 9; a++)
		{
			for (int b = 0; b < 9; b++)
			{
				if (b != a)
				{
					for (int c = 0; c < 9; c++)
					{
						if (c != b && c != a)
						{
							for (int d = 0; d < 9; d++)
							{
								if (d != c && d != b && d != a)
								{
									for (int e = 0; e < 9; e++)
									{
										if (e != d && e != c && e != b && e != a)
										{
											for (int f = 0; f < 9; f++)
											{
												if (f != e && f != d && f != c && f != b && f != a)
												{
													for (int g = 0; g < 9; g++)
													{
														if (g != f && g != e && g != d && g != c && g != b && g != a)
														{
															for (int h = 0; h < 9; h++)
															{
																if (h != g && h != f && h != e && h != d && h != c && h != b && h != a)
																{
																	for (int i = 0; i < 9; i++)
																	{
																		if (i != h && i != g && i != f && i != e && i != d && i != c && i != b && i != a)
																		{
																			for(int j = 0; j < 4; j++)
																			{
																				vReverse[0, 0, j] = v[a / 3, a % 3, j];
																				vReverse[0, 1, j] = v[b / 3, b % 3, j];
																				vReverse[0, 2, j] = v[c / 3, c % 3, j];
																				vReverse[1, 0, j] = v[d / 3, d % 3, j];
																				vReverse[1, 1, j] = v[e / 3, e % 3, j];
																				vReverse[1, 2, j] = v[f / 3, f % 3, j];
																				vReverse[2, 0, j] = v[g / 3, g % 3, j];
																				vReverse[2, 1, j] = v[h / 3, h % 3, j];
																				vReverse[2, 2, j] = v[i / 3, i % 3, j];
																			}
																			if (Finished(vReverse))
                                                                            {
                                                                                breakk = true;
																				for (int j = 0; j < vReverses.Count; j++)
																				{
																					if (EqualV(vReverse, vReverses[j]))
																					{
                                                                                        breakk = false;
																						break;
																					}
																				}
																				if (breakk)
																				{
                                                                                    vReverses.Add(new int[3, 3, 4]);
																					Array.Copy(vReverse, vReverses[^1], 36);
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
        return vReverses;
	}

	bool EqualV(int[,,] v1, int[,,] v2)
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					if (v1[i, j, k] != v2[i, j, k])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

    void AddMove(List<int[,,]> all, List<int> prevDir, List<string> path)
    {
        List<int[,,]> allNew = new ();
		int[] emptyPos;
        List<int> prevDirNew = new ();
        List<string> pathNew = new ();
        for (int i = 0; i < all.Count; i++)
        {
            emptyPos = Empty(all[i]);
            if (emptyPos[0] != 0 && prevDir[i] != 2 && (prevDir[i] != 5 || all[i][emptyPos[0] - 1, emptyPos[1], 0] != -2))
            {
                prevDirNew.Add(0);
                pathNew.Add(path[i] + "v ");
                allNew.Add(Move(all[i], emptyPos, emptyPos[0] - 1, emptyPos[1]));
            }
            if (emptyPos[1] != 2 && prevDir[i] != 3 && (prevDir[i] != 5 || all[i][emptyPos[0], emptyPos[1] + 1, 0] != -2))
            {
                prevDirNew.Add(1);
                pathNew.Add(path[i] + "< ");
                allNew.Add(Move(all[i], emptyPos, emptyPos[0], emptyPos[1] + 1));
            }
            if (emptyPos[0] != 2 && prevDir[i] != 0 && (prevDir[i] != 5 || all[i][emptyPos[0] + 1 ,emptyPos[1], 0] != -2))
            {
                prevDirNew.Add(2);
                pathNew.Add(path[i] + "^ ");
                allNew.Add(Move(all[i], emptyPos, emptyPos[0] + 1, emptyPos[1]));
            }
            if (emptyPos[1] != 0 && prevDir[i] != 1 && (prevDir[i] != 5 || all[i][emptyPos[0], emptyPos[1] - 1, 0] != -2))
            {
                prevDirNew.Add(3);
                pathNew.Add(path[i] + "> ");
                allNew.Add(Move(all[i], emptyPos, emptyPos[0], emptyPos[1] - 1));
            }
		}
		prevDir.Clear();
		prevDir.AddRange(prevDirNew);
		path.Clear();
		path.AddRange(pathNew);
		all.Clear();
		all.AddRange(allNew);
	}

	int[,,] Move(int[,,] v, int[] emptyPos, int newPosX, int newPosY)
	{
		int[,,] vvv = new int[3, 3, 4];
		Array.Copy(v, vvv, 36);
		for(int i = 0; i < 4; i++)
		{
			int vv = vvv[emptyPos[0], emptyPos[1], i];
			vvv[emptyPos[0], emptyPos[1], i] = vvv[newPosX, newPosY, i];
			vvv[newPosX, newPosY, i] = vv;
		}
		return vvv;
	}

	int[] Empty(int[,,] v)
	{
		int[] emptyPos = new int[2];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (v[i,j,0] == -1)
				{
					emptyPos[0] = i;
					emptyPos[1] = j;
					break;
				}
			}
		}
		return emptyPos;
	}

	string FixPath(string path)
	{
		string pathNew = "";
		for (int i = path.Count() - 2; i >= 0; i--)
		{
            pathNew += path[i] switch
            {
                'v' => '^',
                '>' => '<',
                '^' => 'v',
                '<' => '>',
                _ => ' ',
            };
        }
		return pathNew;
	}

	public static void ButtonClicked(Material material)
    {
        if(selected)
        {
            selectedObject.GetComponent<MeshRenderer>().material = material;
            Vector3 v = selectedObject.transform.position;
            t[(int)(v.z / -1.41f + 1), (int)(v.x / 1.41f + 1)] = int.Parse(material.name);
        }
    }
    public void Update()
    {
        if(selected)
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				if (selectedObject.transform.position.z != 1.41f)
				{
					Blocks[SelectedIndex() - 3].transform.position = selectedObject.transform.position;
					selectedObject.transform.position += new Vector3(0, 0, 1.41f);
					GameObject tempObj = Blocks[SelectedIndex() + 3];
					Blocks[SelectedIndex() + 3] = Blocks[SelectedIndex()];
					Blocks[SelectedIndex()] = tempObj;
					int temp = t[SelectedIndex() / 3, SelectedIndex() % 3];
					t[SelectedIndex() / 3, SelectedIndex() % 3] = t[SelectedIndex() / 3 + 1, SelectedIndex() % 3];
					t[SelectedIndex() / 3 + 1, SelectedIndex() % 3] = temp;
					temp = r[SelectedIndex() / 3, SelectedIndex() % 3];
					r[SelectedIndex() / 3, SelectedIndex() % 3] = r[SelectedIndex() / 3 + 1, SelectedIndex() % 3];
					r[SelectedIndex() / 3 + 1, SelectedIndex() % 3] = temp;
				}
			}
			else if (Input.GetKeyDown(KeyCode.A))
			{
				if (selectedObject.transform.position.x != -1.41f)
				{
					Blocks[SelectedIndex() - 1].transform.position = selectedObject.transform.position;
					selectedObject.transform.position += new Vector3(-1.41f, 0, 0);
					GameObject tempObj = Blocks[SelectedIndex() + 1];
					Blocks[SelectedIndex() + 1] = Blocks[SelectedIndex()];
					Blocks[SelectedIndex()] = tempObj;
					int temp = t[SelectedIndex() / 3, SelectedIndex() % 3];
					t[SelectedIndex() / 3, SelectedIndex() % 3] = t[SelectedIndex() / 3, SelectedIndex() % 3 + 1];
					t[SelectedIndex() / 3, SelectedIndex() % 3 + 1] = temp;
					temp = r[SelectedIndex() / 3, SelectedIndex() % 3];
					r[SelectedIndex() / 3, SelectedIndex() % 3] = r[SelectedIndex() / 3, SelectedIndex() % 3 + 1];
					r[SelectedIndex() / 3, SelectedIndex() % 3 + 1] = temp;
				}
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				if (selectedObject.transform.position.z != -1.41f)
				{
					Blocks[SelectedIndex() + 3].transform.position = selectedObject.transform.position;
					selectedObject.transform.position += new Vector3(0, 0, -1.41f);
					GameObject tempObj = Blocks[SelectedIndex() - 3];
					Blocks[SelectedIndex() - 3] = Blocks[SelectedIndex()];
					Blocks[SelectedIndex()] = tempObj;
					int temp = t[SelectedIndex() / 3, SelectedIndex() % 3];
					t[SelectedIndex() / 3, SelectedIndex() % 3] = t[SelectedIndex() / 3 - 1, SelectedIndex() % 3];
					t[SelectedIndex() / 3 - 1, SelectedIndex() % 3] = temp;
					temp = r[SelectedIndex() / 3, SelectedIndex() % 3];
					r[SelectedIndex() / 3, SelectedIndex() % 3] = r[SelectedIndex() / 3 - 1, SelectedIndex() % 3];
					r[SelectedIndex() / 3 - 1, SelectedIndex() % 3] = temp;
				}
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				if (selectedObject.transform.position.x != 1.41f)
				{
					Blocks[SelectedIndex() + 1].transform.position = selectedObject.transform.position;
					selectedObject.transform.position += new Vector3(1.41f, 0, 0);
					GameObject tempObj = Blocks[SelectedIndex() - 1];
					Blocks[SelectedIndex() - 1] = Blocks[SelectedIndex()];
					Blocks[SelectedIndex()] = tempObj;
					int temp = t[SelectedIndex() / 3, SelectedIndex() % 3];
					t[SelectedIndex() / 3, SelectedIndex() % 3] = t[SelectedIndex() / 3, SelectedIndex() % 3 - 1];
					t[SelectedIndex() / 3, SelectedIndex() % 3 - 1] = temp;
					temp = r[SelectedIndex() / 3, SelectedIndex() % 3];
					r[SelectedIndex() / 3, SelectedIndex() % 3] = r[SelectedIndex() / 3, SelectedIndex() % 3 - 1];
					r[SelectedIndex() / 3, SelectedIndex() % 3 - 1] = temp;
				}
			}
		}
    }
	private int SelectedIndex()
    {
		return ((int)(selectedObject.transform.position.z / -1.41f) + 1) * 3 + ((int)(selectedObject.transform.position.x / 1.41f)) + 1;
	}
	public void Quit()
    {
		Application.Quit();
    }
}