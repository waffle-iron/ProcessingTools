﻿namespace ProcessingTools.Globals
{
    public interface ITagger
    {
        void Tag();

        void Tag(IXPathProvider xpathProvider);
    }
}
