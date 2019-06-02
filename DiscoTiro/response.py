class response():
    def __init__(self, cancerchanse, noncancerchanse):
        self.cancerchanse = float(cancerchanse) * 100
        self.noncancerchanse = float(noncancerchanse) * 100

    def get_chanse(self):
        return self.cancerchanse

    def get_rest(self):
        return self.noncancerchanse