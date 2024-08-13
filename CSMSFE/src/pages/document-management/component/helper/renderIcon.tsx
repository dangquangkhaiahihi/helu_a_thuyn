
import DocIcon from '@/utils/icon-jsx/DocIcon';
import XlsxIcon from '@/utils/icon-jsx/XlsxIcon';
import JpgIcon from '@/utils/icon-jsx/JpgIcon';
import PngIcon from '@/utils/icon-jsx/PngIcon';
import GifIcon from '@/utils/icon-jsx/GifIcon';
import PdfIcon from '@/utils/icon-jsx/PdfIcon';
import TifIcon from '@/utils/icon-jsx/TiffIcon';
import TxtIcon from '@/utils/icon-jsx/TxtIcon';
import DescriptionOutlinedIcon from '@mui/icons-material/DescriptionOutlined';

export const renderIcon = (extension: string) => {
    switch(extension) {
        case ".doc":
        case ".docx":
            return <DocIcon />
        case ".xls":
        case ".xlsx":
            return <XlsxIcon />
        case ".png":
            return <PngIcon />
        case ".gif":
            return <GifIcon />
        case ".jpeg":
        case ".jpg":
            return <JpgIcon />
        case ".tiff":
            return <TifIcon />
        case ".pdf":
            return <PdfIcon />
        case ".txt":
            return <TxtIcon />
        default:
            return <DescriptionOutlinedIcon fontSize='small' />
    }
}