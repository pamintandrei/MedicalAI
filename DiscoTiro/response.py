class response():
    def __init__(self, cancerchanse, noncancerchanse):
        self.cancerchanse = cancerchanse
        self.noncancerchanse = noncancerchanse

    def get_chanse(self):
        return self.cancerchanse

    def get_rest(self):
        return self.noncancerchanse