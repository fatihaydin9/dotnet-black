using Black.Infrastructure.Helpers.MailHelper.Elements;
using System.Text;

namespace Black.Infrastructure.Helpers.MailHelper.Methods;

public class MailBlock
{
    private List<Func<string>> _commands = new List<Func<string>>();
    private MailBodyTemplate _template;
    private MailBlock _footer;
    private bool _isBlock;

    public MailBlock(MailBodyTemplate template, MailBlock footer, bool isBlock)
    {
        _template = template;
        _footer = footer;
        _isBlock = isBlock;
    }

    /// <summary>
    /// Add a new title.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public MailBlock Title(string content, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ContentElement
            {
                Content = MailBody.HtmlEncode(content),
                Attributes = attributes
            };
            return _template.Title()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a new subtitle.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public MailBlock SubTitle(string content, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ContentElement
            {
                Content = MailBody.HtmlEncode(content),
                Attributes = attributes
            };
            return _template.SubTitle()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a new paragraph.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public MailBlock Paragraph(string content, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ContentElement
            {
                Content = MailBody.HtmlEncode(content),
                Attributes = attributes
            };
            return _template.Paragraph()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a new paragraph
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    public MailBlock Paragraph(MailBlock block, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            // Propagate template.
            block._template = this._template;

            var element = new ContentElement
            {
                Content = block.ToString()
            };
            return _template.Paragraph()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a new link
    /// </summary>
    /// <param name="link"></param>
    /// <returns></returns>
    public MailBlock Link(string link, dynamic attributes = null)
    {
        return Link(link, link);
    }

    /// <summary>
    /// Add a new link.
    /// </summary>
    /// <param name="link"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public MailBlock Link(string link, string text, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ActionElement
            {
                Content = MailBody.HtmlEncode(text),
                Link = MailBody.AttributeEncode(link),
                Attributes = attributes
            };
            return _template.Link()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a button with a link.
    /// </summary>
    /// <param name="link"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public MailBlock Button(string link, string text, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ActionElement
            {
                Content = MailBody.HtmlEncode(text),
                Link = MailBody.AttributeEncode(link),
                Attributes = attributes
            };
            return _template.Button()(element);
        });
        return this;
    }

    /// <summary>
    /// Add plain text.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public MailBlock Text(string text, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ContentElement
            {
                Content = MailBody.HtmlEncode(text),
                Attributes = attributes
            };
            return _template.Text()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a strong text.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public MailBlock StrongText(string text, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ContentElement
            {
                Content = MailBody.HtmlEncode(text),
                Attributes = attributes
            };
            return _template.StrongText()(element);
        });
        return this;
    }

    /// <summary>
    /// Add raw html. This allows you to add custom html.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public MailBlock Raw(string html)
    {
        _commands.Add(() =>
        {
            return html;
        });
        return this;
    }

    /// <summary>
    /// Add a new break line.
    /// </summary>
    /// <returns></returns>
    public MailBlock LineBreak(dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ContentElement
            {
                Content = string.Empty,
                Attributes = attributes
            };
            return _template.LineBreak()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a unordered list.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public MailBlock UnorderedList(IEnumerable<MailBlock> items, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var builder = new StringBuilder();
            foreach (var item in items)
            {
                var itemElement = new ContentElement { Content = MailBody.HtmlEncode(item.ToString()), Attributes = attributes };
                builder.Append(_template.ListItem()(itemElement));
            }
            var element = new ContentElement { Content = builder.ToString(), Attributes = attributes };
            return _template.UnorderedList()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a ordered list.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public MailBlock OrderedList(IEnumerable<MailBlock> items, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var builder = new StringBuilder();
            foreach (var item in items)
            {
                var itemElement = new ContentElement { Content = MailBody.HtmlEncode(item.ToString()), Attributes = attributes };
                builder.Append(_template.ListItem()(itemElement));
            }
            var element = new ContentElement { Content = builder.ToString(), Attributes = attributes };
            return _template.OrderedList()(element);
        });
        return this;
    }

    /// <summary>
    /// Add a block list.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public MailBlock AddBlocksList(IEnumerable<MailBlock> items)
    {
        _commands.Add(() =>
        {
            var builder = new StringBuilder();
            foreach (var item in items)
            {
                // Propagate template.
                item._template = this._template;

                builder.Append(item.ToString());
            }
            return builder.ToString();
        });
        return this;
    }

    /// <summary>
    /// Generate the html.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => GenerateHtml();

    /// <summary>
    /// Generate the html
    /// </summary>
    /// <returns></returns>
    public string GenerateHtml(dynamic attributes = null)
    {
        if (_template == null)
        {
            _template = MailBodyTemplate.GetDefaultTemplate();
        }

        var builder = new StringBuilder();
        foreach (var command in _commands)
        {
            builder.Append(command());
        }

        if (_isBlock)
        {
            var element = new ContentElement
            {
                Content = builder.ToString(),
                Attributes = attributes
            };
            return _template.Block()(element);
        }
        else
        {
            if (_footer != null)
            {
                _footer._template = this._template;
            }

            var element = new BodyElement
            {
                Content = builder.ToString(),
                Attributes = attributes,
                Footer = _footer != null ? _footer.ToString() : string.Empty
            };
            return _template.Body()(element);
        }
    }

    public MailBlock Image(string link, string alternativeText, dynamic attributes = null)
    {
        _commands.Add(() =>
        {
            var element = new ImageElement
            {
                Content = MailBody.HtmlEncode(alternativeText),
                Src = MailBody.AttributeEncode(link),
                Attributes = attributes
            };
            return _template.Image()(element);
        });
        return this;
    }
}