class personaldata:

    def __init__(self, sex, age, on_thyroxine, query_on_thyroxin, on_antythyroid_medication, thyroid_surgery, query_hypothyroid, query_hyperthyroid, pregnant, sick, tumor, lithium, goitre, TSH_measured, TSH, T3_measured, T3, TT4_measured, TT4, FTI_measured, FTI, TBG_measured, TBG):
        self.Sex = sex
        self.Age = age
        self.on_thyroxine = on_thyroxine
        self.query_on_thyroxine = query_on_thyroxin
        self.on_antithyroid_medication = on_antythyroid_medication
        self.thyroid_surgery = thyroid_surgery
        self.query_hypothyroid = query_hypothyroid
        self.query_hyperthyroid = query_hyperthyroid
        self.pregnant = pregnant
        self.sick = sick
        self.tumor = tumor
        self.lithium = lithium
        self.goitre = goitre
        self.TSH_measured = TSH_measured
        self.TSH = TSH
        self.T3_measured = T3_measured
        self.T3 = T3
        self.TT4_measured = TT4_measured
        self.TT4 = TT4
        self.FTI_measured = FTI_measured
        self.FTI = FTI
        self.TBG_measured = TBG_measured
        self.TBG = TBG
        self.action = "analize"
        self.cookie = ""
        self.patient_name = ""
