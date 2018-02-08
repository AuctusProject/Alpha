export class AdvisorWizardStep {

    public static readonly Start = new AdvisorWizardStep(0, "Start");
    public static readonly Advisor = new AdvisorWizardStep(1, "Advisor");
    public static readonly Portfolio = new AdvisorWizardStep(2, "Portfolio");

    public Id: number;
    public Name: string;

    constructor(id: number, name: string) {
        this.Id = id;
        this.Name = name;
    }

    public static List() {
        return [
            this.Start,
            this.Advisor,
            this.Portfolio
        ]
    }
}
