from __future__ import annotations

from pathlib import Path
import re


DOCS_ROOT = Path("docs-src")
WIKI_ROOT = DOCS_ROOT / "wiki"
OUTPUT_FILE = Path("mkdocs.yml")
IGNORED_DIRECTORIES = {".attachments", "attachments", ".plantuml"}
ACRONYMS = {
    "api": "API",
    "certs": "Certificates",
    "db": "DB",
    "hmlr": "HMLR",
    "id": "ID",
    "landregistry": "Land Registry",
    "oauth2": "OAuth2",
    "openapi": "OpenAPI",
    "soap": "SOAP",
}
LOWERCASE_WORDS = {"a", "an", "and", "by", "for", "in", "of", "on", "or", "the", "to"}


def yaml_quote(value: str) -> str:
    return "'" + value.replace("'", "''") + "'"


def read_order(directory: Path) -> list[str]:
    order_file = directory / ".order"
    if not order_file.exists():
        return []

    items: list[str] = []
    for line in order_file.read_text(encoding="utf-8").splitlines():
        entry = line.strip()
        if entry:
            items.append(entry)
    return items


def split_words(value: str) -> list[str]:
    value = re.sub(r"^\d+[-_ ]*", "", value)
    value = value.replace("_", "-")
    value = re.sub(r"(?<=[a-z0-9])(?=[A-Z])", " ", value)
    value = re.sub(r"(?<=[A-Z])(?=[A-Z][a-z])", " ", value)
    value = value.replace("-", " ")
    return [token for token in value.split() if token]


def to_label(name: str, parent_name: str | None = None) -> str:
    label_source = name

    if parent_name:
        for separator in ("-", "_"):
            prefix = f"{parent_name}{separator}"
            if label_source.lower().startswith(prefix.lower()):
                label_source = label_source[len(prefix):]
                break

    words = split_words(label_source)
    formatted: list[str] = []

    for word in words:
        lower_word = word.lower()
        if lower_word in ACRONYMS:
            formatted.append(ACRONYMS[lower_word])
        elif word.isupper():
            formatted.append(word)
        elif lower_word in LOWERCASE_WORDS and formatted:
            formatted.append(lower_word)
        else:
            formatted.append(word[:1].upper() + word[1:])

    return " ".join(formatted) or name


def sort_key(name: str, files: dict[str, Path], directories: dict[str, Path]) -> tuple[int, str]:
    has_file = name in files
    has_directory = name in directories

    if has_file and has_directory:
        group_rank = 0
    elif has_file:
        group_rank = 1
    else:
        group_rank = 2

    return group_rank, name.lower()


def relative_doc_path(path: Path) -> str:
    return path.relative_to(DOCS_ROOT).as_posix()


def build_nav(directory: Path, parent_name: str | None = None) -> list[tuple[str, str | list]]:
    files = {
        path.stem: path
        for path in directory.iterdir()
        if path.is_file() and path.suffix.lower() == ".md"
    }
    directories = {
        path.name: path
        for path in directory.iterdir()
        if path.is_dir() and path.name not in IGNORED_DIRECTORIES and not path.name.startswith(".")
    }

    ordered_names: list[str] = []
    seen: set[str] = set()

    for name in read_order(directory):
        if name in seen or (name not in files and name not in directories):
            continue
        ordered_names.append(name)
        seen.add(name)

    remaining_names = sorted(
        [name for name in set(files) | set(directories) if name not in seen],
        key=lambda name: sort_key(name, files, directories),
    )

    nav: list[tuple[str, str | list]] = []
    for name in ordered_names + remaining_names:
        has_file = name in files
        has_directory = name in directories
        label = to_label(name, parent_name)

        if has_directory:
            child_nav = build_nav(directories[name], name)
            if has_file:
                if child_nav:
                    nav.append((label, [("Overview", relative_doc_path(files[name])), *child_nav]))
                else:
                    nav.append((label, relative_doc_path(files[name])))
            elif child_nav:
                nav.append((label, child_nav))
        elif has_file:
            nav.append((label, relative_doc_path(files[name])))

    return nav


def append_nav_lines(lines: list[str], items: list[tuple[str, str | list]], indent: int = 2) -> None:
    for label, target in items:
        prefix = " " * indent
        if isinstance(target, str):
            lines.append(f"{prefix}- {yaml_quote(label)}: {yaml_quote(target)}")
            continue

        lines.append(f"{prefix}- {yaml_quote(label)}:")
        append_nav_lines(lines, target, indent + 4)


def main() -> None:
    nav = [("About", "index.md")]
    if WIKI_ROOT.exists():
        nav.extend(build_nav(WIKI_ROOT))

    lines = [
        "site_name: POC - Land Registry",
        "docs_dir: docs-src",
        "site_dir: site",
        "theme:",
        "  name: material",
        "plugins:",
        "  - search",
        "  - roamlinks",
        "nav:",
    ]
    append_nav_lines(lines, nav)
    lines.extend(
        [
            "markdown_extensions:",
            "  - toc:",
            "      permalink: true",
            "  - pymdownx.superfences",
            "  - plantuml_markdown:",
            "      server: https://www.plantuml.com/plantuml",
            "      format: svg",
        ]
    )

    OUTPUT_FILE.write_text("\n".join(lines) + "\n", encoding="utf-8")


if __name__ == "__main__":
    main()