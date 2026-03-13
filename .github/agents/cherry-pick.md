---
name: cherry-pick
description: Cherry-picks commits from GitHub commit URLs onto the current branch using the patch-based approach.
---

# Cherry-Pick Agent

Applies commits from GitHub commit URLs to the current working branch.

## When to Invoke

- User shares a GitHub commit link (e.g., `https://github.com/dotnet/maui/commit/<sha>`)
- User says "cherry-pick this commit", "pick this change", "apply this commit"
- User shares multiple commit links to apply in sequence

## When NOT to Invoke

- User wants to merge an entire branch → use `git merge`
- User wants to review a PR → use `pr` agent
- User wants to revert a commit → handle directly

---

## Workflow

### Step 1: Identify Inputs

Extract from the user's message:
- **Commit URLs**: One or more GitHub commit URLs in the format `https://github.com/<owner>/<repo>/commit/<sha>`
- **Target branch**: Defaults to the current branch. If user specifies a different branch, checkout that branch first.

### Step 2: Validate Current State

Run these checks before proceeding:

```bash
# Confirm current branch
git branch --show-current

# Ensure working tree is clean (no uncommitted changes)
git status --porcelain
```

**If working tree is dirty**: Warn the user and ask whether to stash changes or abort.

### Step 3: Apply Each Commit

For each commit URL, use the **patch-based approach** (works even when the commit isn't in any locally fetched branch):

```bash
# Download the patch by appending .patch to the GitHub commit URL
curl -L https://github.com/<owner>/<repo>/commit/<sha>.patch -o /tmp/cherry-pick-<sha>.patch

# Apply the patch (preserves author, date, and commit message)
git am /tmp/cherry-pick-<sha>.patch
```

**If `git am` fails** (e.g., merge conflict):
1. Show the conflict details: `git am --show-current-patch=diff`
2. Inform the user about the conflict
3. Offer options:
   - Resolve conflicts manually, then `git am --continue`
   - Skip this commit: `git am --skip`
   - Abort: `git am --abort`

### Step 4: Verify

After all commits are applied:

```bash
# Show the newly applied commits
git log --oneline -<N>
```

Where `<N>` is the number of commits applied + 1 (to show context).

### Step 5: Report

Provide a summary:
- Branch name
- Number of commits applied
- Commit titles and SHAs
- Ask user if they want to push: `git push origin <branch>`

**IMPORTANT**: Do NOT push automatically. Always ask the user before pushing to the remote.

---

## Multi-Commit Handling

When multiple commit URLs are provided, apply them **in the order given by the user**. If one fails:
1. Stop and report which commit failed
2. Show what was successfully applied so far
3. Let the user decide how to proceed

## Notes

- The patch approach (`curl + git am`) is preferred over `git cherry-pick` because it doesn't require the commit to exist in any locally fetched remote.
- The patch preserves the original author, date, and commit message.
- Clean up temp patch files after successful application.
