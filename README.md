# �ثe���Ϊ��޳N���H�U�X�I:

## �[�c����

1. �Ҷq�ݨD�����רä������A�ҥH�ΤFMinimal API���覡�Ӷ}�o

2. �ج[�ϥΤFClean Architecture���覡�Ӷ}�o

    - Model��T�h
	    - ViewModel�G �ΨӳB�z��UI����ƭ�
		- ServiceModel�G �ΨӳB�z�޿譱
		- DataLayerModel�G �ΨӳB�z��ƭ�
	- �o�˪��n�B�O�i�H���C�@�h��¾�d����T�A�]�i�H���C�@�h���椸���է�e��
	- Minimal API�t�d�B�zUI�������AService�t�d�B�z�޿誺�����ADataLayer�t�d�B�z��Ʀs��������

3. �ΤFDependency Injection���覡�Ӻ޲z���󪺥ͩR�g��
4. ��Ʈw�ϥΤFEntity Framework Core (LocalDB)
5. �h�y�t�������A�ڨϥΤFCultureInfo.CurrentCulture.Name�Ө��o�y�t�A�åB��@�F��ؼg�k(�ҹ�@�bService�h)
	- �Ĥ@�جO��ƪ�
	    - �ڶ}�o�F�y�t����ƪ�A�N���O�M�y�t�ɫئ��F�D�q�����
		- ��ı�o�o�˪��覡�����ı�A�]����e�����@(���O�}�oUI�i��޲z�B���B��s����)
		- �bQueryCoindesk��API�̥i�H�ݨ�AMethod�W�٬�ConvertCurrencyNameByDB
    - �ĤG�جO�귽��
	    - �ڹ�@�F�h�y�t��Resx�ɡA�bQueryCoindesk��API�̥i�H�ݨ�AMethod�W�٬�ConvertCurrencyNameByResx
6. �ϥΤFSingleton���覡�ӫإߧ֨�
	- �]���d�߹��O���ʧ@�O²��B���Ъ��A���F�ײv�|���ܰʥ~�A���O��������ӬO�T�w���A�ҥH�ڱN�d�߹��O�����G�ظm�֨�
	- ���ײv�O���ɮĩʪ��A�ҥH�ڷ|�N�ײv����Ƨ֨��_�ӡA�åB�]�w�@�Ӯɶ�(�ثe�]��30��)�A�W�L�ɶ��N���s�d��
	- �֨����ظm�A�ڨϥ�MemoryCache
	- ��@�bQueryCurrencyInfo���o��API��

## Error Handling����

1. UI�h
    - �t�d�B�z��ƨӷ�������A�Y�����~�A�h�ߥX�L�o�����~�T��
	    - �إߤ@�Ӧ۩w�q��ApiResponseViewModel�A�Τ@��X���~�T��
2. Service�h
	- �t�d�B�z�޿誺�����A�Y�����~�A�h�^���ŭȩΪŶ��X
		- �qUI�өάOService�h�Ӫ��d�ߡA�ҥi�w���^�ǵ��G
		- ���h�Ҧ��@�Ӷ}�o�W�d�h�[�Wtry...catch�A�H�T�O�^�Ǩ�w�������G
3. Provider�h
	- �t�d�B�z��Ʀs���������A�o�����h�����ҥ~�B�z
		- ���h�Ҷq�g��Controller�h�L�o�L����ơA�w�q�L����
		- Service�h�w�g�[�W�B�z���~�������AProvider�h�u�����`�B�z��Ʀs�����@�~�Y�i

## ��Ʈw����

1. ��Ʈw�ϥΤFEntity Framework Core (LocalDB)
2. ��ƪ�Ҽ{��h�y�t���ݨD�A�ҥH�ڱN���O�M�y�t���}�A���F�D�q�����
3. �qController�h���ѤF�y�t(langKey From CultureInfo.CurrentCulture.Name)���ӷ��A�]��K��������y�t�ɡA����CultureInfo.CurrentCulture�Y�i

## ���ճ���
1. �椸���ժ������A�|������
    - �b�\�h���M�׸g��U�A�`�]���M�׶}�o�ɵ{�����O�A�椸���ձ`�`�Q����
	- �ϥΤFxUnit�Ӱ��椸���ժ��g��O���֪��A�ҥH�|������