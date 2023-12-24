using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode2023;
public class Day24 : IDay
{
    public int Day => 24;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "19, 13, 30 @ -2,  1, -2\r\n18, 19, 22 @ -1, -1, -2\r\n20, 25, 34 @ -2, -2, -4\r\n12, 31, 28 @ -1, -2, -1\r\n20, 19, 15 @  1, -5, -3",
            "2"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "19, 13, 30 @ -2,  1, -2\r\n18, 19, 22 @ -1, -1, -2\r\n20, 25, 34 @ -2, -2, -4\r\n12, 31, 28 @ -1, -2, -1\r\n20, 19, 15 @  1, -5, -3",
            "47"
        },

    };

    public string SolvePart1(string input)
    {
        long total = 0;


        var allNums = Utils.GetLongs(input).ToArray();
        List<(double px, double py, double pz, double vx, double vy, double vz)> hailstones = new();

        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(' ').Where((n, i) => n != "" && i != 3).Select(n => double.Parse(n.TrimEnd(','))).ToList();

            hailstones.Add((line[0], line[1], line[2], line[3], line[4], line[5]));
        }

        long min = UnitTestsP1.ContainsKey(input) ? 7 : 200000000000000;
        long max = UnitTestsP1.ContainsKey(input) ? 27 : 400000000000000;

        long count = 0;
        foreach (var combo in hailstones.Combinations(2).Select(c => c.ToList()))
        {
            // combo[0].px + k * combo[0].vx = combo[1].px + t * combo[1].vx
            // combo[0].py + k * combo[0].vy = combo[1].py + t * combo[1].vy

            // x  -(1 / combo[0].vy)
            // -combo[0].py / combo[0].vy - k * combo[0].vy / combo[0].vy = -combo[1].py / combo[0].vy + t * combo[1].vy / combo[0].vy

            // x combo[0].vx
            // -combo[0].py * combo[0].vx / combo[0].vy - k * combo[0].vx = -combo[1].py * combo[0].vx / combo[0].vy + t * combo[1].vy * combo[0].vx / combo[0].vy


            // sum

            // combo[0].px + k * combo[0].vx = combo[1].px + t * combo[1].vx
            // -combo[0].py * combo[0].vx / combo[0].vy - k * combo[0].vx = -combo[1].py * combo[0].vx / combo[0].vy - t * combo[1].vy * combo[0].vx / combo[0].vy

            // combo[0].px - combo[0].py * combo[0].vx / combo[0].vy  = combo[1].px - combo[1].py * combo[0].vx / combo[0].vy + t * (combo[1].vx - combo[1].vy * combo[0].vx / combo[0].vy)


            // combo[0].px - combo[0].py * combo[0].vx / combo[0].vy + combo[1].py * combo[0].vx / combo[0].vy - combo[1].px = t * (combo[1].vx - combo[1].vy * combo[0].vx / combo[0].vy)

            // t = (combo[0].px - combo[0].py * combo[0].vx / combo[0].vy + combo[1].py * combo[0].vx / combo[0].vy - combo[1].px) / (combo[1].vx - combo[1].vy * combo[0].vx / combo[0].vy)


            var ratio = combo[0].vx / combo[0].vy;
            var denominator = (combo[1].vx - combo[1].vy * ratio);


            if (denominator != 0)
            {
                var t = (combo[0].px - combo[0].py * ratio + combo[1].py * ratio - combo[1].px) / denominator;

                // k * combo[0].vx = combo[1].px - combo[0].px + t * combo[1].vx
                //k = (combo[1].px - combo[0].px + t * combo[1].vx) / combo[0].vx
                var k = (combo[1].px - combo[0].px + t * combo[1].vx) / combo[0].vx;

                if (t > 0 && k > 0)
                {
                    var pos = (X: combo[1].px + t * combo[1].vx, Y: combo[1].py + t * combo[1].vy, Z: combo[1].pz + t * combo[1].vz);

                    if (min <= pos.X && pos.X <= max && min <= pos.Y && pos.Y <= max)
                    {
                        count++;
                    }

                }

                /*
                // check in final equation
                

                */
            }
        }


        return $"{count}";
    }

    public string SolvePart2(string input)
    {
        var allNums = Utils.GetLongs(input).ToArray();
        List<(double px, double py, double pz, double vx, double vy, double vz)> hailstones = new();

        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var data = lines[i].Split(' ').Where((n, i) => n != "" && i != 3).Select(n => double.Parse(n.TrimEnd(','))).ToList();
            hailstones.Add((data[0], data[1], data[2], data[3], data[4], data[5]));
        }

        var (px1, py1, pz1, vx1, vy1, vz1) = hailstones[0];
        var (px2, py2, pz2, vx2, vy2, vz2) = hailstones[1];

        const int range = 200;

        for (int i = -range; i < range; i++)
        {
            for (int j = -range; j < range; j++)
            {
                /*
                var mat = new Matrix(new double[,]
                {
                    { i - vx1, vx2 - i },
                    { j - vy1, vy2 - j }
                });

                var det = mat.Determinant;
                if (det == 0)
                    continue;

                var ans = 1 / det * mat.Adjugate * new Matrix(new double[,]
                {
                    { px1 - px2 },
                    { py1 - py2 }
                });

                var t1 = ans[0, 0];
                var t2 = ans[1, 0];*/

                // solved algebraically
                var t2 = ((i - vx1) * (py1 - py2) - (j - vy1) * (px1 - px2)) / ((i - vx1) * (vy2 - j) - (j - vy1) * (px2 - i));
                var t1 = (px1 - px2 + (i - vx2) * t2) / (i - vx1);

                var x = Math.Round(px1 + (vx1 - i) * t1);
                var y = Math.Round(py1 + (vy1 - j) * t1);

                /*
                var mat2 = new Matrix(new double[,]
                {
                    { 1, t1 },
                    { 1, t2 }
                });

                var det2 = mat2.Determinant;
                if (det2 == 0)
                    continue;

                var ans2 = 1 / det2 * mat2.Adjugate * new Matrix(new double[,]
                {
                    { pz1 + vz1 * t1 },
                    { pz2 + vz2 * t2 }
                });

                //var z = Math.Round(ans2[0, 0], MidpointRounding.AwayFromZero);
                //var c = Math.Round(ans2[1, 0], MidpointRounding.AwayFromZero);

                */

                // simults solved algebraically
                var k = Math.Round((pz2 + vz2 * t2 - pz1 - vz1 * t1) / (t2 - t1));
                var z = Math.Round(pz1 + (vz1 - k) * t1);

                (double px, double py, double pz, double vx, double vy, double vz) line = (x, y, z, i, j, k);

                //
                //if (hailstones.All(l => LinesIntersect2(ToArr(line), ToArr(l))))
                if (hailstones.Where((l, x) => x != i && x != j).All(l => LinesIntersect(line, l)))
                {
                    return $"{line.px + line.py + line.pz}";
                }
            }
        }

        return "Not found within estimated range";
    }

 
    static bool LinesIntersect((double px, double py, double pz, double vx, double vy, double vz) l1, (double px, double py, double pz, double vx, double vy, double vz) l2)
    {
        var ratio = l1.vx / l1.vy;
        var denominator = l2.vx - l2.vy * ratio; 

        if (denominator == 0) return true;

        var t = (l1.px - l1.py * ratio + l2.py * ratio - l2.px) / denominator;
        var k = (l2.px - l1.px + t * l2.vx) / l1.vx;

        //return l1.pz + k * l1.vz == l2.pz + t * l2.vz;
        return Equal(l1.pz + k * l1.vz, l2.pz + t * l2.vz);

        // if (t < 0 || k < 0) return false;
        //intersectionPoint = (X: l2.px + t * l2.vx, Y: l2.py + t * l2.vy, Z: l2.pz + t * l2.vz);

    }

    static double[] ToArr((double px, double py, double pz, double vx, double vy, double vz) data)
        => new double[] {data.px, data.py, data.pz, data.vx, data.vy, data.vz};



    static bool Equal(double a, double b) => Math.Abs(a - b) <= 0.000001;
}