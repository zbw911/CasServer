using System;
using NCAS.jasig.web.MOCK2JAVA;

public class ModelAndView
{
    private string p;
    private RedirectView redirectView;
    private string p1;
    private string p2;
    private string p3;

    public ModelAndView(string p)
    {
        // TODO: Complete member initialization
        this.p = p;
    }

    public ModelAndView(RedirectView redirectView)
    {
        // TODO: Complete member initialization
        this.redirectView = redirectView;
    }

    public ModelAndView(string p1, string p2, string p3)
    {
        // TODO: Complete member initialization
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }
    public void addObject(string code, object p1)
    {
        throw new NotImplementedException();
    }
}